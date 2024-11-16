using EtcsServer.Database;
using EtcsServer.Database.Entity;
using EtcsServer.InMemoryHolders;

namespace EtcsServer.MapLoading
{
    public class UnityTrackageMap : ITrackageMap
    {
        public Dictionary<int, Crossing> CrossingsLookup { get; set; }
        public Dictionary<int, CrossingTrack> CrossingTracksLookup { get; set; }
        public Dictionary<int, RailroadSign> RailroadSignsLookup { get; set; }
        public Dictionary<int, RailwaySignal> RailwaySignalsLookup { get; set; }
        public Dictionary<int, SwitchRoute> SwitchRoutesLookup { get; set; }
        public Dictionary<int, Track> TracksLookup { get; set; }
        public Dictionary<int, TrackageElement> TrackageElementsLookup { get; set; }

        public UnityTrackageMap()
        {
            PrepareDataForInsert();
        }

        public void PrepareDataForInsert()
        {
            double switchingTrackLength = 0.07;
            double shortBetweenSwitches = 0.1;
            double longBetweenSwitches = 0.2;
            double lowSwitchingSpeed = 40;
            double highSwitchingSpeed = 60;

            List<TrackageElement> trackageElements = [
                new Switch() {
                    TrackageElementId = 111,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 112,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 113,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 114,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 115,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 116,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 121,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 122,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 123,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 124,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 131,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 132,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 133,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 134,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 141,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 142,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 143,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 144,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 151,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 152,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 153,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 154,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 155,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 156,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 157,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 158,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Track() {
                    TrackageElementId = 1,
                    LeftSideElementId = null,
                    RightSideElementId = 111,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 0,
                    Length = 0.2,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 2,
                    LeftSideElementId = null,
                    RightSideElementId = 112,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 0,
                    Length = 0.2,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 3,
                    LeftSideElementId = null,
                    RightSideElementId = 116,
                    LineNumber = 1,
                    TrackNumber = "4",
                    Kilometer = 0,
                    Length = 0.17,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 240,
                    LeftSideElementId = 116,
                    RightSideElementId = 112,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 4,
                    LeftSideElementId = 112,
                    RightSideElementId = 113,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 0.2,
                    Length = 0.05,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 5,
                    LeftSideElementId = 111,
                    RightSideElementId = 115,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 0.2,
                    Length = longBetweenSwitches,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 6,
                    LeftSideElementId = 111,
                    RightSideElementId = 113,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 7,
                    LeftSideElementId = 113,
                    RightSideElementId = 114,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 0.25,
                    Length = shortBetweenSwitches,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 8,
                    LeftSideElementId = 114,
                    RightSideElementId = 115,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 209,
                    LeftSideElementId = 115,
                    RightSideElementId = 9,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 0.4,
                    Length = 0.1,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 9,
                    LeftSideElementId = 209,
                    RightSideElementId = 211,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 0.5,
                    Length = 1.3,
                    MaxUpSpeed = 150,
                    MaxDownSpeed = 150,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 211,
                    LeftSideElementId = 9,
                    RightSideElementId = 121,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 1.8,
                    Length = 1.65,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 210,
                    LeftSideElementId = 114,
                    RightSideElementId = 10,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 0.35,
                    Length = 0.15,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 10,
                    LeftSideElementId = 210,
                    RightSideElementId = 213,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 0.5,
                    Length = 1.3,
                    MaxUpSpeed = 150,
                    MaxDownSpeed = 150,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 213,
                    LeftSideElementId = 10,
                    RightSideElementId = 122,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 1.8,
                    Length = 1.7,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 11,
                    LeftSideElementId = 121,
                    RightSideElementId = 124,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 3.45,
                    Length = longBetweenSwitches,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 12,
                    LeftSideElementId = 121,
                    RightSideElementId = 122,
                    Length = switchingTrackLength,
                    MaxSpeed = highSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 13,
                    LeftSideElementId = 122,
                    RightSideElementId = 123,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 3.5,
                    Length = shortBetweenSwitches,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 14,
                    LeftSideElementId = 123,
                    RightSideElementId = 124,
                    Length = switchingTrackLength,
                    MaxSpeed = highSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 15,
                    LeftSideElementId = 124,
                    RightSideElementId = 131,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 3.65,
                    Length = 5.396,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 16,
                    LeftSideElementId = 123,
                    RightSideElementId = 132,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 3.6,
                    Length = 5.496,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 17,
                    LeftSideElementId = 131,
                    RightSideElementId = 134,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 9.046,
                    Length = longBetweenSwitches,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 18,
                    LeftSideElementId = 131,
                    RightSideElementId = 132,
                    Length = switchingTrackLength,
                    MaxSpeed = highSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 19,
                    LeftSideElementId = 132,
                    RightSideElementId = 133,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 9.096,
                    Length = shortBetweenSwitches,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 20,
                    LeftSideElementId = 133,
                    RightSideElementId = 134,
                    Length = switchingTrackLength,
                    MaxSpeed = highSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 221,
                    LeftSideElementId = 134,
                    RightSideElementId = 21,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 9.246,
                    Length = 0.1,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 21,
                    LeftSideElementId = 221,
                    RightSideElementId = 224,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 9.346,
                    Length = 1.1,
                    MaxUpSpeed = 140,
                    MaxDownSpeed = 140,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 224,
                    LeftSideElementId = 21,
                    RightSideElementId = 226,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 10.446,
                    Length = 0.02,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 226,
                    LeftSideElementId = 224,
                    RightSideElementId = 228,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 10.466,
                    Length = 0.02,
                    MaxUpSpeed = 60,
                    MaxDownSpeed = 60,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 228,
                    LeftSideElementId = 226,
                    RightSideElementId = 230,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 10.486,
                    Length = 1.11,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 120,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 230,
                    LeftSideElementId = 228,
                    RightSideElementId = 141,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 11.596,
                    Length = 0.1,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 222,
                    LeftSideElementId = 133,
                    RightSideElementId = 22,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 9.196,
                    Length = 0.15,
                    MaxUpSpeed = 160,
                    MaxDownSpeed = 160,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 22,
                    LeftSideElementId = 222,
                    RightSideElementId = 223,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 9.346,
                    Length = 1.1,
                    MaxUpSpeed = 140,
                    MaxDownSpeed = 140,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 223,
                    LeftSideElementId = 22,
                    RightSideElementId = 225,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 10.446,
                    Length = 0.02,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 40,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 225,
                    LeftSideElementId = 223,
                    RightSideElementId = 227,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 10.466,
                    Length = 0.02,
                    MaxUpSpeed = 60,
                    MaxDownSpeed = 60,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 227,
                    LeftSideElementId = 225,
                    RightSideElementId = 229,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 10.486,
                    Length = 1.11,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 120,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 229,
                    LeftSideElementId = 227,
                    RightSideElementId = 142,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 11.596,
                    Length = 0.15,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 23,
                    LeftSideElementId = 141,
                    RightSideElementId = 144,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 11.696,
                    Length = longBetweenSwitches,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 24,
                    LeftSideElementId = 141,
                    RightSideElementId = 142,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 25,
                    LeftSideElementId = 142,
                    RightSideElementId = 143,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 11.746,
                    Length = shortBetweenSwitches,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 26,
                    LeftSideElementId = 143,
                    RightSideElementId = 144,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 27,
                    LeftSideElementId = 144,
                    RightSideElementId = 237,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 11.896,
                    Length = 1.75,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },

                new Track() {
                    TrackageElementId = 237,
                    LeftSideElementId = 27,
                    RightSideElementId = 151,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 13.646,
                    Length = 0.15,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 28,
                    LeftSideElementId = 143,
                    RightSideElementId = 228,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 11.846,
                    Length = 1.8,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 238,
                    LeftSideElementId = 28,
                    RightSideElementId = 152,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 13.646,
                    Length = 0.2,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 29,
                    LeftSideElementId = 151,
                    RightSideElementId = 154,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 13.796,
                    Length = longBetweenSwitches,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 30,
                    LeftSideElementId = 151,
                    RightSideElementId = 152,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 31,
                    LeftSideElementId = 152,
                    RightSideElementId = 153,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 13.846,
                    Length = shortBetweenSwitches,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 32,
                    LeftSideElementId = 153,
                    RightSideElementId = 154,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 33,
                    LeftSideElementId = 154,
                    RightSideElementId = 155,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 13.996,
                    Length = 0.1,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 34,
                    LeftSideElementId = 153,
                    RightSideElementId = 156,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 13.946,
                    Length = 0.15,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 241,
                    LeftSideElementId = 155,
                    RightSideElementId = 157,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 242,
                    LeftSideElementId = 156,
                    RightSideElementId = 158,
                    Length = switchingTrackLength,
                    MaxSpeed = lowSwitchingSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 35,
                    LeftSideElementId = 157,
                    RightSideElementId = null,
                    LineNumber = 1,
                    TrackNumber = "3",
                    Kilometer = 14.146,
                    Length = 0.2,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 36,
                    LeftSideElementId = 155,
                    RightSideElementId = null,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 14.096,
                    Length = 0.25,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 37,
                    LeftSideElementId = 156,
                    RightSideElementId = null,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 14.096,
                    Length = 0.25,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 38,
                    LeftSideElementId = 158,
                    RightSideElementId = null,
                    LineNumber = 1,
                    TrackNumber = "4",
                    Kilometer = 14.146,
                    Length = 0.2,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
            ];

            TrackageElementsLookup = trackageElements.ToDictionary(te => te.TrackageElementId, te => te);
            TracksLookup = trackageElements.Where(te => te is Track).ToDictionary(te => te.TrackageElementId, te => (Track)te);

            RailroadSignsLookup = [];

            RailwaySignalsLookup = new()
            {
                { 1, new() { RailwaySignalId = 1, TrackId = 1, Track = TracksLookup[1], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 2, new() { RailwaySignalId = 2, TrackId = 2, Track = TracksLookup[2], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 4, new() { RailwaySignalId = 4, TrackId = 4, Track = TracksLookup[4], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 11, new() { RailwaySignalId = 11, TrackId = 1, Track = TracksLookup[1], DistanceFromTrackStart = 0.15, IsFacedUp = true } },
                { 22, new() { RailwaySignalId = 22, TrackId = 2, Track = TracksLookup[2], DistanceFromTrackStart = 0.15, IsFacedUp = true } },
                { 44, new() { RailwaySignalId = 44, TrackId = 4, Track = TracksLookup[4], DistanceFromTrackStart = 0.15, IsFacedUp = true } },
                { 9209, new() { RailwaySignalId = 9209, TrackId = 9, Track = TracksLookup[9], DistanceFromTrackStart = TracksLookup[9].Length, IsFacedUp = false } },
                { 10210, new() { RailwaySignalId = 10210, TrackId = 10, Track = TracksLookup[10], DistanceFromTrackStart = TracksLookup[10].Length, IsFacedUp = false } },
                { 213, new() { RailwaySignalId = 213, TrackId = 213, Track = TracksLookup[213], DistanceFromTrackStart = 1.55, IsFacedUp = true } },
                { 211, new() { RailwaySignalId = 211, TrackId = 211, Track = TracksLookup[211], DistanceFromTrackStart = 1.55, IsFacedUp = true } },
                { 15, new() { RailwaySignalId = 15, TrackId = 15, Track = TracksLookup[15], DistanceFromTrackStart = 0.1, IsFacedUp = false } },
                { 16, new() { RailwaySignalId = 16, TrackId = 16, Track = TracksLookup[16], DistanceFromTrackStart = 0.15, IsFacedUp = false } },
                { 1515, new() { RailwaySignalId = 1515, TrackId = 15, Track = TracksLookup[15], DistanceFromTrackStart = 5.298, IsFacedUp = true } },
                { 1616, new() { RailwaySignalId = 1616, TrackId = 16, Track = TracksLookup[16], DistanceFromTrackStart = 5.348, IsFacedUp = true } },
                { 22222, new() { RailwaySignalId = 22222, TrackId = 22, Track = TracksLookup[22], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 21221, new() { RailwaySignalId = 21221, TrackId = 21, Track = TracksLookup[21], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 227229, new() { RailwaySignalId = 227229, TrackId = 227, Track = TracksLookup[227], DistanceFromTrackStart = TracksLookup[227].Length, IsFacedUp = true } },
                { 228230, new() { RailwaySignalId = 228230, TrackId = 228, Track = TracksLookup[228], DistanceFromTrackStart = TracksLookup[228].Length, IsFacedUp = true } },
                { 28, new() { RailwaySignalId = 28, TrackId = 28, Track = TracksLookup[28], DistanceFromTrackStart = 0.15, IsFacedUp = false } },
                { 27, new() { RailwaySignalId = 27, TrackId = 27, Track = TracksLookup[27], DistanceFromTrackStart = 0.1, IsFacedUp = false } },
                { 28238, new() { RailwaySignalId = 28238, TrackId = 28, Track = TracksLookup[28], DistanceFromTrackStart = TracksLookup[28].Length, IsFacedUp = false } },
                { 27237, new() { RailwaySignalId = 27237, TrackId = 27, Track = TracksLookup[27], DistanceFromTrackStart = TracksLookup[27].Length, IsFacedUp = false } },
                { 3838, new() { RailwaySignalId = 3838, TrackId = 38, Track = TracksLookup[38], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 3737, new() { RailwaySignalId = 3737, TrackId = 37, Track = TracksLookup[37], DistanceFromTrackStart = 0.05, IsFacedUp = false } },
                { 3636, new() { RailwaySignalId = 3636, TrackId = 36, Track = TracksLookup[36], DistanceFromTrackStart = 0.05, IsFacedUp = false } },
                { 3535, new() { RailwaySignalId = 3535, TrackId = 35, Track = TracksLookup[35], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 38, new() { RailwaySignalId = 38, TrackId = 38, Track = TracksLookup[38], DistanceFromTrackStart = TracksLookup[38].Length, IsFacedUp = true } },
                { 37, new() { RailwaySignalId = 37, TrackId = 37, Track = TracksLookup[37], DistanceFromTrackStart = TracksLookup[37].Length, IsFacedUp = true } },
                { 36, new() { RailwaySignalId = 36, TrackId = 36, Track = TracksLookup[36], DistanceFromTrackStart = TracksLookup[36].Length, IsFacedUp = true } },
                { 35, new() { RailwaySignalId = 35, TrackId = 35, Track = TracksLookup[35], DistanceFromTrackStart = TracksLookup[35].Length, IsFacedUp = true } },
            };

            SwitchRoutesLookup = new()
            {
                {1, new() { SwitchRouteId = 1, SwitchId = 111, Switch = (Switch)TrackageElementsLookup[111], TrackFromId = 1, TrackFrom = TrackageElementsLookup[1], TrackToId = 5, TrackTo = TrackageElementsLookup[5] } },
                {2, new() { SwitchRouteId = 2, SwitchId = 111, Switch = (Switch)TrackageElementsLookup[111], TrackFromId = 1, TrackFrom = TrackageElementsLookup[1], TrackToId = 6, TrackTo = TrackageElementsLookup[6] } },
                {3, new() { SwitchRouteId = 3, SwitchId = 111, Switch = (Switch)TrackageElementsLookup[111], TrackFromId = 5, TrackFrom = TrackageElementsLookup[5], TrackToId = 1, TrackTo = TrackageElementsLookup[1] } },
                {4, new() { SwitchRouteId = 4, SwitchId = 111, Switch = (Switch)TrackageElementsLookup[111], TrackFromId = 6, TrackFrom = TrackageElementsLookup[6], TrackToId = 1, TrackTo = TrackageElementsLookup[1] } },
                {95, new() { SwitchRouteId = 95, SwitchId = 112, Switch = (Switch)TrackageElementsLookup[112], TrackFromId = 2, TrackFrom = TrackageElementsLookup[2], TrackToId = 4, TrackTo = TrackageElementsLookup[4] } },
                {96, new() { SwitchRouteId = 96, SwitchId = 112, Switch = (Switch)TrackageElementsLookup[112], TrackFromId = 240, TrackFrom = TrackageElementsLookup[240], TrackToId = 4, TrackTo = TrackageElementsLookup[4] } },
                {97, new() { SwitchRouteId = 97, SwitchId = 112, Switch = (Switch)TrackageElementsLookup[112], TrackFromId = 4, TrackFrom = TrackageElementsLookup[4], TrackToId = 2, TrackTo = TrackageElementsLookup[2] } },
                {98, new() { SwitchRouteId = 98, SwitchId = 112, Switch = (Switch)TrackageElementsLookup[112], TrackFromId = 4, TrackFrom = TrackageElementsLookup[4], TrackToId = 240, TrackTo = TrackageElementsLookup[240] } },
                {5, new() { SwitchRouteId = 5, SwitchId = 113, Switch = (Switch)TrackageElementsLookup[113], TrackFromId = 4, TrackFrom = TrackageElementsLookup[4], TrackToId = 7, TrackTo = TrackageElementsLookup[7] } },
                {6, new() { SwitchRouteId = 6, SwitchId = 113, Switch = (Switch)TrackageElementsLookup[113], TrackFromId = 6, TrackFrom = TrackageElementsLookup[6], TrackToId = 7, TrackTo = TrackageElementsLookup[7] } },
                {7, new() { SwitchRouteId = 7, SwitchId = 113, Switch = (Switch)TrackageElementsLookup[113], TrackFromId = 7, TrackFrom = TrackageElementsLookup[7], TrackToId = 4, TrackTo = TrackageElementsLookup[4] } },
                {8, new() { SwitchRouteId = 8, SwitchId = 113, Switch = (Switch)TrackageElementsLookup[113], TrackFromId = 7, TrackFrom = TrackageElementsLookup[7], TrackToId = 6, TrackTo = TrackageElementsLookup[6] } },
                {9, new() { SwitchRouteId = 9, SwitchId = 114, Switch = (Switch)TrackageElementsLookup[114], TrackFromId = 7, TrackFrom = TrackageElementsLookup[7], TrackToId = 210, TrackTo = TrackageElementsLookup[210] } },
                {10, new() { SwitchRouteId = 10, SwitchId = 114, Switch = (Switch)TrackageElementsLookup[114], TrackFromId = 7, TrackFrom = TrackageElementsLookup[7], TrackToId = 8, TrackTo = TrackageElementsLookup[8] } },
                {11, new() { SwitchRouteId = 11, SwitchId = 114, Switch = (Switch)TrackageElementsLookup[114], TrackFromId = 210, TrackFrom = TrackageElementsLookup[210], TrackToId = 7, TrackTo = TrackageElementsLookup[7] } },
                {12, new() { SwitchRouteId = 12, SwitchId = 114, Switch = (Switch)TrackageElementsLookup[114], TrackFromId = 8, TrackFrom = TrackageElementsLookup[8], TrackToId = 7, TrackTo = TrackageElementsLookup[7] } },
                {13, new() { SwitchRouteId = 13, SwitchId = 115, Switch = (Switch)TrackageElementsLookup[115], TrackFromId = 5, TrackFrom = TrackageElementsLookup[5], TrackToId = 209, TrackTo = TrackageElementsLookup[209] } },
                {14, new() { SwitchRouteId = 14, SwitchId = 115, Switch = (Switch)TrackageElementsLookup[115], TrackFromId = 8, TrackFrom = TrackageElementsLookup[8], TrackToId = 209, TrackTo = TrackageElementsLookup[209] } },
                {15, new() { SwitchRouteId = 15, SwitchId = 115, Switch = (Switch)TrackageElementsLookup[115], TrackFromId = 209, TrackFrom = TrackageElementsLookup[209], TrackToId = 5, TrackTo = TrackageElementsLookup[5] } },
                {16, new() { SwitchRouteId = 16, SwitchId = 115, Switch = (Switch)TrackageElementsLookup[115], TrackFromId = 209, TrackFrom = TrackageElementsLookup[209], TrackToId = 8, TrackTo = TrackageElementsLookup[8] } },
                {17, new() { SwitchRouteId = 17, SwitchId = 116, Switch = (Switch)TrackageElementsLookup[116], TrackFromId = 3, TrackFrom = TrackageElementsLookup[3], TrackToId = 240, TrackTo = TrackageElementsLookup[240] } },
                {18, new() { SwitchRouteId = 18, SwitchId = 116, Switch = (Switch)TrackageElementsLookup[116], TrackFromId = 240, TrackFrom = TrackageElementsLookup[240], TrackToId = 3, TrackTo = TrackageElementsLookup[3] } },
                {19, new() { SwitchRouteId = 19, SwitchId = 121, Switch = (Switch)TrackageElementsLookup[121], TrackFromId = 211, TrackFrom = TrackageElementsLookup[211], TrackToId = 11, TrackTo = TrackageElementsLookup[11] } },
                {20, new() { SwitchRouteId = 20, SwitchId = 121, Switch = (Switch)TrackageElementsLookup[121], TrackFromId = 211, TrackFrom = TrackageElementsLookup[211], TrackToId = 12, TrackTo = TrackageElementsLookup[12] } },
                {21, new() { SwitchRouteId = 21, SwitchId = 121, Switch = (Switch)TrackageElementsLookup[121], TrackFromId = 11, TrackFrom = TrackageElementsLookup[11], TrackToId = 211, TrackTo = TrackageElementsLookup[211] } },
                {22, new() { SwitchRouteId = 22, SwitchId = 121, Switch = (Switch)TrackageElementsLookup[121], TrackFromId = 12, TrackFrom = TrackageElementsLookup[12], TrackToId = 211, TrackTo = TrackageElementsLookup[211] } },
                {23, new() { SwitchRouteId = 23, SwitchId = 122, Switch = (Switch)TrackageElementsLookup[122], TrackFromId = 213, TrackFrom = TrackageElementsLookup[213], TrackToId = 13, TrackTo = TrackageElementsLookup[13] } },
                {24, new() { SwitchRouteId = 24, SwitchId = 122, Switch = (Switch)TrackageElementsLookup[122], TrackFromId = 12, TrackFrom = TrackageElementsLookup[12], TrackToId = 13, TrackTo = TrackageElementsLookup[13] } },
                {25, new() { SwitchRouteId = 25, SwitchId = 122, Switch = (Switch)TrackageElementsLookup[122], TrackFromId = 13, TrackFrom = TrackageElementsLookup[13], TrackToId = 12, TrackTo = TrackageElementsLookup[12] } },
                {26, new() { SwitchRouteId = 26, SwitchId = 122, Switch = (Switch)TrackageElementsLookup[122], TrackFromId = 13, TrackFrom = TrackageElementsLookup[13], TrackToId = 213, TrackTo = TrackageElementsLookup[213] } },
                {27, new() { SwitchRouteId = 27, SwitchId = 123, Switch = (Switch)TrackageElementsLookup[123], TrackFromId = 13, TrackFrom = TrackageElementsLookup[13], TrackToId = 14, TrackTo = TrackageElementsLookup[14] } },
                {28, new() { SwitchRouteId = 28, SwitchId = 123, Switch = (Switch)TrackageElementsLookup[123], TrackFromId = 13, TrackFrom = TrackageElementsLookup[13], TrackToId = 16, TrackTo = TrackageElementsLookup[16] } },
                {29, new() { SwitchRouteId = 29, SwitchId = 123, Switch = (Switch)TrackageElementsLookup[123], TrackFromId = 14, TrackFrom = TrackageElementsLookup[14], TrackToId = 13, TrackTo = TrackageElementsLookup[13] } },
                {30, new() { SwitchRouteId = 30, SwitchId = 123, Switch = (Switch)TrackageElementsLookup[123], TrackFromId = 16, TrackFrom = TrackageElementsLookup[16], TrackToId = 13, TrackTo = TrackageElementsLookup[13] } },
                {31, new() { SwitchRouteId = 31, SwitchId = 124, Switch = (Switch)TrackageElementsLookup[124], TrackFromId = 15, TrackFrom = TrackageElementsLookup[15], TrackToId = 11, TrackTo = TrackageElementsLookup[11] } },
                {32, new() { SwitchRouteId = 32, SwitchId = 124, Switch = (Switch)TrackageElementsLookup[124], TrackFromId = 15, TrackFrom = TrackageElementsLookup[15], TrackToId = 14, TrackTo = TrackageElementsLookup[14] } },
                {33, new() { SwitchRouteId = 33, SwitchId = 124, Switch = (Switch)TrackageElementsLookup[124], TrackFromId = 11, TrackFrom = TrackageElementsLookup[11], TrackToId = 15, TrackTo = TrackageElementsLookup[15] } },
                {34, new() { SwitchRouteId = 34, SwitchId = 124, Switch = (Switch)TrackageElementsLookup[124], TrackFromId = 14, TrackFrom = TrackageElementsLookup[14], TrackToId = 15, TrackTo = TrackageElementsLookup[15] } },
                {35, new() { SwitchRouteId = 35, SwitchId = 131, Switch = (Switch)TrackageElementsLookup[131], TrackFromId = 15, TrackFrom = TrackageElementsLookup[15], TrackToId = 17, TrackTo = TrackageElementsLookup[17] } },
                {36, new() { SwitchRouteId = 36, SwitchId = 131, Switch = (Switch)TrackageElementsLookup[131], TrackFromId = 15, TrackFrom = TrackageElementsLookup[15], TrackToId = 18, TrackTo = TrackageElementsLookup[18] } },
                {37, new() { SwitchRouteId = 37, SwitchId = 131, Switch = (Switch)TrackageElementsLookup[131], TrackFromId = 17, TrackFrom = TrackageElementsLookup[17], TrackToId = 15, TrackTo = TrackageElementsLookup[15] } },
                {38, new() { SwitchRouteId = 38, SwitchId = 131, Switch = (Switch)TrackageElementsLookup[131], TrackFromId = 18, TrackFrom = TrackageElementsLookup[18], TrackToId = 15, TrackTo = TrackageElementsLookup[15] } },
                {39, new() { SwitchRouteId = 39, SwitchId = 132, Switch = (Switch)TrackageElementsLookup[132], TrackFromId = 19, TrackFrom = TrackageElementsLookup[19], TrackToId = 16, TrackTo = TrackageElementsLookup[16] } },
                {40, new() { SwitchRouteId = 40, SwitchId = 132, Switch = (Switch)TrackageElementsLookup[132], TrackFromId = 19, TrackFrom = TrackageElementsLookup[19], TrackToId = 18, TrackTo = TrackageElementsLookup[18] } },
                {41, new() { SwitchRouteId = 41, SwitchId = 132, Switch = (Switch)TrackageElementsLookup[132], TrackFromId = 16, TrackFrom = TrackageElementsLookup[16], TrackToId = 19, TrackTo = TrackageElementsLookup[19] } },
                {42, new() { SwitchRouteId = 42, SwitchId = 132, Switch = (Switch)TrackageElementsLookup[132], TrackFromId = 18, TrackFrom = TrackageElementsLookup[18], TrackToId = 19, TrackTo = TrackageElementsLookup[19] } },
                {43, new() { SwitchRouteId = 43, SwitchId = 133, Switch = (Switch)TrackageElementsLookup[133], TrackFromId = 19, TrackFrom = TrackageElementsLookup[19], TrackToId = 20, TrackTo = TrackageElementsLookup[20] } },
                {44, new() { SwitchRouteId = 44, SwitchId = 133, Switch = (Switch)TrackageElementsLookup[133], TrackFromId = 19, TrackFrom = TrackageElementsLookup[19], TrackToId = 222, TrackTo = TrackageElementsLookup[222] } },
                {45, new() { SwitchRouteId = 45, SwitchId = 133, Switch = (Switch)TrackageElementsLookup[133], TrackFromId = 20, TrackFrom = TrackageElementsLookup[20], TrackToId = 19, TrackTo = TrackageElementsLookup[19] } },
                {46, new() { SwitchRouteId = 46, SwitchId = 133, Switch = (Switch)TrackageElementsLookup[133], TrackFromId = 222, TrackFrom = TrackageElementsLookup[222], TrackToId = 19, TrackTo = TrackageElementsLookup[19] } },
                {47, new() { SwitchRouteId = 47, SwitchId = 134, Switch = (Switch)TrackageElementsLookup[134], TrackFromId = 221, TrackFrom = TrackageElementsLookup[221], TrackToId = 17, TrackTo = TrackageElementsLookup[17] } },
                {48, new() { SwitchRouteId = 48, SwitchId = 134, Switch = (Switch)TrackageElementsLookup[134], TrackFromId = 221, TrackFrom = TrackageElementsLookup[221], TrackToId = 20, TrackTo = TrackageElementsLookup[20] } },
                {49, new() { SwitchRouteId = 49, SwitchId = 134, Switch = (Switch)TrackageElementsLookup[134], TrackFromId = 17, TrackFrom = TrackageElementsLookup[17], TrackToId = 221, TrackTo = TrackageElementsLookup[221] } },
                {50, new() { SwitchRouteId = 50, SwitchId = 134, Switch = (Switch)TrackageElementsLookup[134], TrackFromId = 20, TrackFrom = TrackageElementsLookup[20], TrackToId = 221, TrackTo = TrackageElementsLookup[221] } },
                {51, new() { SwitchRouteId = 51, SwitchId = 141, Switch = (Switch)TrackageElementsLookup[141], TrackFromId = 230, TrackFrom = TrackageElementsLookup[230], TrackToId = 23, TrackTo = TrackageElementsLookup[23] } },
                {52, new() { SwitchRouteId = 52, SwitchId = 141, Switch = (Switch)TrackageElementsLookup[141], TrackFromId = 230, TrackFrom = TrackageElementsLookup[230], TrackToId = 24, TrackTo = TrackageElementsLookup[24] } },
                {53, new() { SwitchRouteId = 53, SwitchId = 141, Switch = (Switch)TrackageElementsLookup[141], TrackFromId = 23, TrackFrom = TrackageElementsLookup[23], TrackToId = 230, TrackTo = TrackageElementsLookup[230] } },
                {54, new() { SwitchRouteId = 54, SwitchId = 141, Switch = (Switch)TrackageElementsLookup[141], TrackFromId = 24, TrackFrom = TrackageElementsLookup[24], TrackToId = 230, TrackTo = TrackageElementsLookup[230] } },
                {55, new() { SwitchRouteId = 55, SwitchId = 142, Switch = (Switch)TrackageElementsLookup[142], TrackFromId = 25, TrackFrom = TrackageElementsLookup[25], TrackToId = 24, TrackTo = TrackageElementsLookup[24] } },
                {56, new() { SwitchRouteId = 56, SwitchId = 142, Switch = (Switch)TrackageElementsLookup[142], TrackFromId = 25, TrackFrom = TrackageElementsLookup[25], TrackToId = 229, TrackTo = TrackageElementsLookup[229] } },
                {57, new() { SwitchRouteId = 57, SwitchId = 142, Switch = (Switch)TrackageElementsLookup[142], TrackFromId = 24, TrackFrom = TrackageElementsLookup[24], TrackToId = 25, TrackTo = TrackageElementsLookup[25] } },
                {58, new() { SwitchRouteId = 58, SwitchId = 142, Switch = (Switch)TrackageElementsLookup[142], TrackFromId = 229, TrackFrom = TrackageElementsLookup[229], TrackToId = 25, TrackTo = TrackageElementsLookup[25] } },
                {59, new() { SwitchRouteId = 59, SwitchId = 143, Switch = (Switch)TrackageElementsLookup[143], TrackFromId = 25, TrackFrom = TrackageElementsLookup[25], TrackToId = 26, TrackTo = TrackageElementsLookup[26] } },
                {60, new() { SwitchRouteId = 60, SwitchId = 143, Switch = (Switch)TrackageElementsLookup[143], TrackFromId = 25, TrackFrom = TrackageElementsLookup[25], TrackToId = 28, TrackTo = TrackageElementsLookup[28] } },
                {61, new() { SwitchRouteId = 61, SwitchId = 143, Switch = (Switch)TrackageElementsLookup[143], TrackFromId = 26, TrackFrom = TrackageElementsLookup[26], TrackToId = 25, TrackTo = TrackageElementsLookup[25] } },
                {62, new() { SwitchRouteId = 62, SwitchId = 143, Switch = (Switch)TrackageElementsLookup[143], TrackFromId = 28, TrackFrom = TrackageElementsLookup[28], TrackToId = 25, TrackTo = TrackageElementsLookup[25] } },
                {63, new() { SwitchRouteId = 63, SwitchId = 144, Switch = (Switch)TrackageElementsLookup[144], TrackFromId = 27, TrackFrom = TrackageElementsLookup[27], TrackToId = 23, TrackTo = TrackageElementsLookup[23] } },
                {64, new() { SwitchRouteId = 64, SwitchId = 144, Switch = (Switch)TrackageElementsLookup[144], TrackFromId = 27, TrackFrom = TrackageElementsLookup[27], TrackToId = 26, TrackTo = TrackageElementsLookup[26] } },
                {65, new() { SwitchRouteId = 65, SwitchId = 144, Switch = (Switch)TrackageElementsLookup[144], TrackFromId = 23, TrackFrom = TrackageElementsLookup[23], TrackToId = 27, TrackTo = TrackageElementsLookup[27] } },
                {66, new() { SwitchRouteId = 66, SwitchId = 144, Switch = (Switch)TrackageElementsLookup[144], TrackFromId = 26, TrackFrom = TrackageElementsLookup[26], TrackToId = 27, TrackTo = TrackageElementsLookup[27] } },
                {67, new() { SwitchRouteId = 67, SwitchId = 151, Switch = (Switch)TrackageElementsLookup[151], TrackFromId = 237, TrackFrom = TrackageElementsLookup[237], TrackToId = 29, TrackTo = TrackageElementsLookup[29] } },
                {68, new() { SwitchRouteId = 68, SwitchId = 151, Switch = (Switch)TrackageElementsLookup[151], TrackFromId = 237, TrackFrom = TrackageElementsLookup[237], TrackToId = 30, TrackTo = TrackageElementsLookup[30] } },
                {69, new() { SwitchRouteId = 69, SwitchId = 151, Switch = (Switch)TrackageElementsLookup[151], TrackFromId = 29, TrackFrom = TrackageElementsLookup[29], TrackToId = 237, TrackTo = TrackageElementsLookup[237] } },
                {70, new() { SwitchRouteId = 70, SwitchId = 151, Switch = (Switch)TrackageElementsLookup[151], TrackFromId = 30, TrackFrom = TrackageElementsLookup[30], TrackToId = 237, TrackTo = TrackageElementsLookup[237] } },
                {71, new() { SwitchRouteId = 71, SwitchId = 152, Switch = (Switch)TrackageElementsLookup[152], TrackFromId = 31, TrackFrom = TrackageElementsLookup[31], TrackToId = 30, TrackTo = TrackageElementsLookup[30] } },
                {72, new() { SwitchRouteId = 72, SwitchId = 152, Switch = (Switch)TrackageElementsLookup[152], TrackFromId = 31, TrackFrom = TrackageElementsLookup[31], TrackToId = 238, TrackTo = TrackageElementsLookup[238] } },
                {73, new() { SwitchRouteId = 73, SwitchId = 152, Switch = (Switch)TrackageElementsLookup[152], TrackFromId = 30, TrackFrom = TrackageElementsLookup[30], TrackToId = 31, TrackTo = TrackageElementsLookup[31] } },
                {74, new() { SwitchRouteId = 74, SwitchId = 152, Switch = (Switch)TrackageElementsLookup[152], TrackFromId = 238, TrackFrom = TrackageElementsLookup[238], TrackToId = 31, TrackTo = TrackageElementsLookup[31] } },
                {75, new() { SwitchRouteId = 75, SwitchId = 153, Switch = (Switch)TrackageElementsLookup[153], TrackFromId = 31, TrackFrom = TrackageElementsLookup[31], TrackToId = 32, TrackTo = TrackageElementsLookup[32] } },
                {76, new() { SwitchRouteId = 76, SwitchId = 153, Switch = (Switch)TrackageElementsLookup[153], TrackFromId = 31, TrackFrom = TrackageElementsLookup[31], TrackToId = 34, TrackTo = TrackageElementsLookup[34] } },
                {77, new() { SwitchRouteId = 77, SwitchId = 153, Switch = (Switch)TrackageElementsLookup[153], TrackFromId = 32, TrackFrom = TrackageElementsLookup[32], TrackToId = 31, TrackTo = TrackageElementsLookup[31] } },
                {78, new() { SwitchRouteId = 78, SwitchId = 153, Switch = (Switch)TrackageElementsLookup[153], TrackFromId = 34, TrackFrom = TrackageElementsLookup[34], TrackToId = 31, TrackTo = TrackageElementsLookup[31] } },
                {79, new() { SwitchRouteId = 79, SwitchId = 154, Switch = (Switch)TrackageElementsLookup[154], TrackFromId = 33, TrackFrom = TrackageElementsLookup[33], TrackToId = 29, TrackTo = TrackageElementsLookup[29] } },
                {80, new() { SwitchRouteId = 80, SwitchId = 154, Switch = (Switch)TrackageElementsLookup[154], TrackFromId = 33, TrackFrom = TrackageElementsLookup[33], TrackToId = 32, TrackTo = TrackageElementsLookup[32] } },
                {81, new() { SwitchRouteId = 81, SwitchId = 154, Switch = (Switch)TrackageElementsLookup[154], TrackFromId = 29, TrackFrom = TrackageElementsLookup[29], TrackToId = 33, TrackTo = TrackageElementsLookup[33] } },
                {82, new() { SwitchRouteId = 82, SwitchId = 154, Switch = (Switch)TrackageElementsLookup[154], TrackFromId = 32, TrackFrom = TrackageElementsLookup[32], TrackToId = 33, TrackTo = TrackageElementsLookup[33] } },
                {83, new() { SwitchRouteId = 83, SwitchId = 155, Switch = (Switch)TrackageElementsLookup[155], TrackFromId = 33, TrackFrom = TrackageElementsLookup[33], TrackToId = 241, TrackTo = TrackageElementsLookup[241] } },
                {84, new() { SwitchRouteId = 84, SwitchId = 155, Switch = (Switch)TrackageElementsLookup[155], TrackFromId = 33, TrackFrom = TrackageElementsLookup[33], TrackToId = 36, TrackTo = TrackageElementsLookup[36] } },
                {85, new() { SwitchRouteId = 85, SwitchId = 155, Switch = (Switch)TrackageElementsLookup[155], TrackFromId = 241, TrackFrom = TrackageElementsLookup[241], TrackToId = 33, TrackTo = TrackageElementsLookup[33] } },
                {86, new() { SwitchRouteId = 86, SwitchId = 155, Switch = (Switch)TrackageElementsLookup[155], TrackFromId = 36, TrackFrom = TrackageElementsLookup[36], TrackToId = 33, TrackTo = TrackageElementsLookup[33] } },
                {87, new() { SwitchRouteId = 87, SwitchId = 156, Switch = (Switch)TrackageElementsLookup[156], TrackFromId = 34, TrackFrom = TrackageElementsLookup[34], TrackToId = 37, TrackTo = TrackageElementsLookup[37] } },
                {88, new() { SwitchRouteId = 88, SwitchId = 156, Switch = (Switch)TrackageElementsLookup[156], TrackFromId = 34, TrackFrom = TrackageElementsLookup[34], TrackToId = 242, TrackTo = TrackageElementsLookup[242] } },
                {89, new() { SwitchRouteId = 89, SwitchId = 156, Switch = (Switch)TrackageElementsLookup[156], TrackFromId = 37, TrackFrom = TrackageElementsLookup[37], TrackToId = 34, TrackTo = TrackageElementsLookup[34] } },
                {90, new() { SwitchRouteId = 90, SwitchId = 156, Switch = (Switch)TrackageElementsLookup[156], TrackFromId = 242, TrackFrom = TrackageElementsLookup[242], TrackToId = 34, TrackTo = TrackageElementsLookup[34] } },
                {91, new() { SwitchRouteId = 91, SwitchId = 157, Switch = (Switch)TrackageElementsLookup[157], TrackFromId = 241, TrackFrom = TrackageElementsLookup[241], TrackToId = 35, TrackTo = TrackageElementsLookup[35] } },
                {92, new() { SwitchRouteId = 92, SwitchId = 157, Switch = (Switch)TrackageElementsLookup[157], TrackFromId = 35, TrackFrom = TrackageElementsLookup[35], TrackToId = 241, TrackTo = TrackageElementsLookup[241] } },
                {93, new() { SwitchRouteId = 93, SwitchId = 158, Switch = (Switch)TrackageElementsLookup[158], TrackFromId = 242, TrackFrom = TrackageElementsLookup[242], TrackToId = 38, TrackTo = TrackageElementsLookup[38] } },
                {94, new() { SwitchRouteId = 94, SwitchId = 158, Switch = (Switch)TrackageElementsLookup[158], TrackFromId = 38, TrackFrom = TrackageElementsLookup[38], TrackToId = 242, TrackTo = TrackageElementsLookup[242] } },
            };


            CrossingsLookup = new()
            {
                {1, new() { CrossingId = 1 } }
            };

            CrossingTracksLookup = new()
            {
                {1, new() { CrossingTrackId = 1, CrossingId = 1, TrackId = 15, Track = TracksLookup[15], DistanceFromTrackStart = 2.698 } },
                {2, new() { CrossingTrackId = 2, CrossingId = 1, TrackId = 16, Track = TracksLookup[16], DistanceFromTrackStart = 2.748 } },
            };
        }
    }
}
