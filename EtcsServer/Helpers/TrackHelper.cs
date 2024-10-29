using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;
using Microsoft.IdentityModel.Protocols.Configuration;

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

        public Track? GetNextTrack(int trackId, bool isDirectionUp)
        {
            Track track = tracksHolder.GetValues()[trackId];
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

        public Track? GetTrackById(int trackId) {
            return tracksHolder.GetValues()[trackId];
        }

        public Track? GetTrackByTrainPosition(TrainPosition trainPosition)
        {
            Track? track = tracksHolder.GetValues().Values
                .Where(t => t.TrackNumber.Equals(trainPosition.Track) && t.LineNumber == trainPosition.LineNumber)
                .Where(t => t.Kilometer <= trainPosition.Kilometer)
                .OrderBy(t => t.Kilometer)
                .LastOrDefault();
            
            if (track == null) return null;
            if (track.Kilometer + track.Length < track.Kilometer)
                return null;

            return track;

        }
    }
}
