using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace EtcsServer.Helpers
{
    public class TrackHelper : ITrackHelper
    {
        private readonly IHolder<Track> tracksHolder;
        private readonly ISwitchStates switchStates;

        public TrackHelper(IHolder<Track> tracksHolder, ISwitchStates switchStates)
        {
            this.tracksHolder = tracksHolder;
            this.switchStates = switchStates;
        }

        public TrackageElement? GetNextTrackageElement(int trackId, bool isDirectionUp)
        {
            Track track = tracksHolder.GetValues()[trackId];
            return isDirectionUp ? track.RightSideElement : track.LeftSideElement;
        }

        public Track? GetNextTrack(int trackId, bool isDirectionUp)
        {
            TrackageElement? nextElement = GetNextTrackageElement(trackId, isDirectionUp);

            switch (nextElement) {
                case null:
                    return null;
                case Switch trainSwitch:
                    int nextTrackId = switchStates.GetNextTrackId(trainSwitch.TrackageElementId, trackId);
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
