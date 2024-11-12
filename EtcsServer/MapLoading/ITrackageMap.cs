using EtcsServer.Database;
using EtcsServer.Database.Entity;
using EtcsServer.InMemoryHolders;

namespace EtcsServer.MapLoading
{
    public interface ITrackageMap
    {
        public Dictionary<int, Crossing> CrossingsLookup { get; set; }
        public Dictionary<int, CrossingTrack> CrossingTracksLookup { get; set; }
        public Dictionary<int, RailroadSign> RailroadSignsLookup { get; set; }
        public Dictionary<int, RailwaySignal> RailwaySignalsLookup { get; set; }
        public Dictionary<int, SwitchRoute> SwitchRoutesLookup { get; set; }
        public Dictionary<int, Track> TracksLookup { get; set; }
        public Dictionary<int, TrackageElement> TrackageElementsLookup { get; set; }

    }
}
