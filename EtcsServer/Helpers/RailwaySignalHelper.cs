using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using Microsoft.IdentityModel.Tokens;

namespace EtcsServer.Helpers
{
    public class RailwaySignalHelper : IRailwaySignalHelper
    {
        private readonly ITrackHelper trackHelper;
        private readonly IHolder<RailwaySignal> railwaySignalTrackHolder;
        private readonly IRailwaySignalStates railwaySignalStates;

        public RailwaySignalHelper(
            ITrackHelper trackHelper,
            IHolder<RailwaySignal> railwaySignalTrackHolder,
            IRailwaySignalStates railwaySignalStates
            )
        {
            this.trackHelper = trackHelper;
            this.railwaySignalTrackHolder = railwaySignalTrackHolder;
            this.railwaySignalStates = railwaySignalStates;
        }

        public RailwaySignal? GetFirstStopSignal(TrainPosition trainPosition, bool isMovingUp)
        {
            Track? currentTrack = trackHelper.GetTrackByTrainPosition(trainPosition);
            double currentKilometer = trainPosition.Kilometer;
            while (currentTrack != null)
            {
                List<RailwaySignal> signalsOnCurrentTrack = railwaySignalTrackHolder.GetValues().Values
                .Where(s => s.TrackId == currentTrack.TrackageElementId)
                .Where(s => s.IsFacedUp == isMovingUp)
                .Where(s => isMovingUp ? currentKilometer <= s.DistanceFromTrackStart : currentKilometer >= s.DistanceFromTrackStart)
                .OrderBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                .ToList();

                RailwaySignal? firstStopSignal = signalsOnCurrentTrack
                    .FirstOrDefault(s => railwaySignalStates.GetSignalMessage(s.RailwaySignalId) == RailwaySignalMessage.STOP);
                if (firstStopSignal != null)
                    return firstStopSignal;

                currentTrack = trackHelper.GetNextTrack(currentTrack.TrackageElementId, isMovingUp);
                if (currentTrack != null)
                    currentKilometer = isMovingUp ? currentTrack.Kilometer : currentTrack.Length;
                else return null;
            }

            return null;
        }

        public RailwaySignalMessage GetMessageForSignal(int signalId) {
            return railwaySignalStates.GetSignalMessage(signalId);
        }
    }
}
