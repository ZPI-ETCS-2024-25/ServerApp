using EtcsServer.Database.Entity;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;

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

        public RailwaySignalTrack GetFirstSignalForTrack(int trackId, bool isMovingUp)
        {
            List<RailwaySignalTrack> signalsForTrack = railwaySignalTrackHolder.GetValues().Values
                .Where(s => s.TrackId == trackId)
                .Where(s => s.IsFacedUp == isMovingUp)
                .OrderBy(s => s.DistanceFromTrackStart)
                .ToList();

            return isMovingUp ? signalsForTrack.First() : signalsForTrack.Last();
        }

        public RailwaySignalMessage GetMessageForSignal(int signalId) {
            return railwaySignalStates.GetSignalMessage(signalId);
        }
    }
}
