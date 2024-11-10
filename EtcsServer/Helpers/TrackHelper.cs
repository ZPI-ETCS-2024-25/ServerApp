using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using Microsoft.IdentityModel.Protocols.Configuration;
using System.Web.Http.Tracing;

namespace EtcsServer.Helpers
{
    public class TrackHelper : ITrackHelper
    {
        private readonly IHolder<TrackageElement> trackageElementsHolder;
        private readonly IHolder<Track> tracksHolder;
        private readonly ISwitchStates switchStates;

        public TrackHelper(
            IHolder<TrackageElement> trackageElementsHolder,
            IHolder<Track> tracksHolder,
            ISwitchStates switchStates
            )
        {
            this.trackageElementsHolder = trackageElementsHolder;
            this.tracksHolder = tracksHolder;
            this.switchStates = switchStates;
        }

        public TrackageElement GetTrackageElement(int trackageElementId)
        {
            return trackageElementsHolder.GetValues()[trackageElementId];
        }

        public TrackageElement? GetNextTrackageElement(int trackageElementId, TrackEnd trackEnd)
        {
            TrackageElement trackageElement = trackageElementsHolder.GetValues()[trackageElementId];
            return trackEnd == TrackEnd.RIGHT ? trackageElement.RightSideElement : trackageElement.LeftSideElement;
        }

        public Track? GetNextTrack(int trackId, MovementDirection movementDirection) => GetNextTrack(trackId, movementDirection == MovementDirection.UP ? TrackEnd.RIGHT : TrackEnd.LEFT);

        public Track? GetNextTrack(int trackId, TrackEnd currentTrackEnd)
        {
            TrackageElement? nextElement = GetNextTrackageElement(trackId, currentTrackEnd);

            switch (nextElement) {
                case null:
                    return null;
                case Switch trainSwitch:
                    return GetNextTrackBySwitch(trainSwitch.TrackageElementId, trackId);
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

        private Track? GetNextTrackBySwitch(int switchId, int trackFromId)
        {
            int nextTrackId = switchStates.GetNextTrackId(switchId, trackFromId);
            TrackageElement trackageElement = GetTrackageElement(nextTrackId);
            if (trackageElement is Track track)
                return track;
            else if (trackageElement is SwitchingTrack switchingTrack)
                return GetNextTrackBySwitchingTrack(switchingTrack, switchId);
            return null;
        }

        private Track? GetNextTrackBySwitchingTrack(SwitchingTrack switchingTrack, int switchFromId)
        {
            TrackageElement? nextElement = switchingTrack.LeftSideElementId == switchFromId ? switchingTrack.RightSideElement : switchingTrack.LeftSideElement;
            if (nextElement != null && nextElement is Switch trainSwitch)
                return GetNextTrackBySwitch(trainSwitch.TrackageElementId, switchingTrack.TrackageElementId);
            else throw new Exception("Switching track " + switchingTrack.TrackageElementId + " is not connected to a switch from both sides");
        }
    }
}
