using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;
using Microsoft.IdentityModel.Tokens;

namespace EtcsServer.Helpers
{
    public class RailwaySignalHelper
    {
        private readonly RailwaySignalTrackHolder railwaySignalTrackHolder;
        private readonly RailwaySignalStates railwaySignalStates;

        public RailwaySignalHelper(RailwaySignalTrackHolder railwaySignalTrackHolder, RailwaySignalStates railwaySignalStates)
        {
            this.railwaySignalTrackHolder = railwaySignalTrackHolder;
            this.railwaySignalStates = railwaySignalStates;
        }

        public RailwaySignalTrack? GetFirstSignalForTrack(TrainPosition trainPosition, int trackIdTo, bool isMovingUp)
        {
            List<RailwaySignalTrack> signalsOnCurrentTrack = railwaySignalTrackHolder.GetValues().Values
                .Where(s => s.Track.TrackNumber.Equals(trainPosition.Track))
                .Where(s => s.IsFacedUp == isMovingUp)
                .Where(s => s.DistanceFromTrackStart > Int32.Parse(trainPosition.Kilometer))
                .OrderBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                .ToList();

            if (signalsOnCurrentTrack.Count > 0)
                return signalsOnCurrentTrack.First();

            List<RailwaySignalTrack> signalsOnNextTrack = railwaySignalTrackHolder.GetValues().Values
                .Where(s => s.TrackId == trackIdTo)
                .Where(s => s.IsFacedUp == isMovingUp)
                .OrderBy(s => s.TrackId == trackIdTo ? 1 : 0)
                .ThenBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                .ToList();

            return signalsOnNextTrack.FirstOrDefault();
        }

        public RailwaySignalMessage GetMessageForSignal(int signalId) {
            return railwaySignalStates.GetSignalMessage(signalId);
        }
    }
}
