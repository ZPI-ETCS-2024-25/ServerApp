using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;
using Microsoft.IdentityModel.Tokens;

namespace EtcsServer.Helpers
{
    public class RailwaySignalHelper
    {
        private readonly TrackHelper trackHelper;
        private readonly RailwaySignalsHolder railwaySignalTrackHolder;
        private readonly RailwaySignalStates railwaySignalStates;

        public RailwaySignalHelper(
            TrackHelper trackHelper,
            RailwaySignalsHolder railwaySignalTrackHolder,
            RailwaySignalStates railwaySignalStates
            )
        {
            this.trackHelper = trackHelper;
            this.railwaySignalTrackHolder = railwaySignalTrackHolder;
            this.railwaySignalStates = railwaySignalStates;
        }

        public RailwaySignal? GetFirstStopSignal(TrainPosition trainPosition, bool isMovingUp)
        {
            int currentTrackId = trackHelper.GetTrackByTrackName(trainPosition.Track)!.TrackageElementId;
            Track? currentTrack = trackHelper.GetTrackById(currentTrackId);
            double currentKilometer = trainPosition.Kilometer;
            while (currentTrack != null)
            {
                List<RailwaySignal> signalsOnCurrentTrack = railwaySignalTrackHolder.GetValues().Values
                .Where(s => s.TrackId == currentTrack.TrackageElementId)
                .Where(s => s.IsFacedUp == isMovingUp)
                .Where(s => isMovingUp ? currentKilometer < s.DistanceFromTrackStart : currentKilometer > s.DistanceFromTrackStart)
                .OrderBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                .ToList();

                RailwaySignal? firstStopSignal = signalsOnCurrentTrack
                    .FirstOrDefault(s => railwaySignalStates.GetSignalMessage(s.RailwaySignalId) != RailwaySignalMessage.STOP);
                if (firstStopSignal != null)
                    return firstStopSignal;

                int? nextTrackId = isMovingUp ? currentTrack.RightSideElementId : currentTrack.LeftSideElementId;
                if (nextTrackId.HasValue)
                {
                    currentTrack = trackHelper.GetTrackById(nextTrackId.Value);
                    if (currentTrack != null)
                        currentKilometer = currentTrack.Kilometer;
                }
                    
                else return null;
            }

            return null;
        }

        public List<RailwaySignal> GetSignalsAheadOfTrain(TrainPosition trainPosition, int trackIdTo, bool isMovingUp)
        {
            List<RailwaySignal> signalsOnCurrentTrack = railwaySignalTrackHolder.GetValues().Values
                .Where(s => s.Track.TrackNumber.Equals(trainPosition.Track) && s.Track.LineNumber == trainPosition.LineNumber)
                .Where(s => s.IsFacedUp == isMovingUp)
                .Where(s => isMovingUp ? trainPosition.Kilometer < s.DistanceFromTrackStart : trainPosition.Kilometer > s.DistanceFromTrackStart)
                .OrderBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                .ToList();

            List<RailwaySignal> signalsOnNextTrack = railwaySignalTrackHolder.GetValues().Values
                .Where(s => s.TrackId == trackIdTo)
                .Where(s => s.IsFacedUp == isMovingUp)
                .OrderBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                .ToList();

            return signalsOnCurrentTrack.Concat(signalsOnNextTrack).ToList();
        }

        public RailwaySignalMessage GetMessageForSignal(int signalId) {
            return railwaySignalStates.GetSignalMessage(signalId);
        }
    }
}
