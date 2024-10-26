using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;

namespace EtcsServer.Helpers
{
    public class TrackHelper
    {
        private readonly TracksHolder tracksHolder;
        private readonly SwitchStates switchStates;

        public TrackHelper(TracksHolder tracksHolder, SwitchStates switchStates)
        {
            this.tracksHolder = tracksHolder;
            this.switchStates = switchStates;
        }

        public Track? GetNextTrack(string trackId, bool isDirectionUp)
        {
            Track track = tracksHolder.GetValues().Values.First(t => t.TrackNumber.Equals(trackId));
            TrackageElement? nextElement = isDirectionUp ? track.RightSideElement : track.LeftSideElement;

            switch (nextElement) {
                case null:
                    return null;
                case Switch trainSwitch:
                    int nextTrackId = switchStates.GetNextTrackId(trainSwitch.TrackageElementId);
                    return GetTrackById(nextTrackId);
                case Track nextTrack:
                    return nextTrack;
                default:
                    return null;
            }
        }

        private Track? GetTrackById(int trackId) {
            return tracksHolder.GetValues()[trackId];
        }
    }
}
