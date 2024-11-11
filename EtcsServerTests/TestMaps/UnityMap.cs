using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryHolders;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtcsServerTests.TestMaps
{
    class UnityMap : MockTestMap
    {
        public TrainDto Train { get; set; }

        public UnityMap()
        {
            this.Train = new()
            {
                TrainId = "97888",
                LengthMeters = "600",
                MaxSpeed = "200",
                BrakeWeight = "1000"
            };
            InitializeHolders();
        }

        protected override void InitializeHolders()
        {

            double switchingTrackSpeed = 20;
            double switchingTrackLength = 0.05;
            double shortBetweenSwitches = 0.1;
            double longBetweenSwitches = 0.2;

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
                    RightSideElementId = 112,
                    LineNumber = 1,
                    TrackNumber = "4",
                    Kilometer = 0,
                    Length = 0.2,
                    MaxUpSpeed = 120,
                    MaxDownSpeed = 40,
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
                    MaxSpeed = switchingTrackSpeed,
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
                    MaxSpeed = switchingTrackSpeed,
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
                    MaxSpeed = switchingTrackSpeed,
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
                    MaxSpeed = switchingTrackSpeed,
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
                    Length = 5.4,
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
                    Length = 5.5,
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
                    Kilometer = 9.05,
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
                    MaxSpeed = switchingTrackSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 19,
                    LeftSideElementId = 132,
                    RightSideElementId = 133,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 9.1,
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
                    MaxSpeed = switchingTrackSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 221,
                    LeftSideElementId = 234,
                    RightSideElementId = 21,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 9.25,
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
                    Kilometer = 9.35,
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
                    Kilometer = 10.45,
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
                    Kilometer = 10.47,
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
                    Kilometer = 10.49,
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
                    Kilometer = 11.6,
                    Length = 0.1,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 222,
                    LeftSideElementId = 233,
                    RightSideElementId = 22,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 9.2,
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
                    Kilometer = 9.35,
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
                    Kilometer = 10.45,
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
                    Kilometer = 10.47,
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
                    Kilometer = 10.49,
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
                    Kilometer = 11.6,
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
                    Kilometer = 11.7,
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
                    MaxSpeed = switchingTrackSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 25,
                    LeftSideElementId = 142,
                    RightSideElementId = 143,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 11.75,
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
                    MaxSpeed = switchingTrackSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 27,
                    LeftSideElementId = 144,
                    RightSideElementId = 227,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 11.9,
                    Length = 1.75,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },

                new Track() {
                    TrackageElementId = 227,
                    LeftSideElementId = 27,
                    RightSideElementId = 151,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 13.65,
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
                    Kilometer = 11.85,
                    Length = 1.8,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 228,
                    LeftSideElementId = 28,
                    RightSideElementId = 152,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 13.65,
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
                    Kilometer = 13.8,
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
                    MaxSpeed = switchingTrackSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 31,
                    LeftSideElementId = 152,
                    RightSideElementId = 153,
                    LineNumber = 1,
                    TrackNumber = "2",
                    Kilometer = 13.85,
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
                    MaxSpeed = switchingTrackSpeed,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 33,
                    LeftSideElementId = 154,
                    RightSideElementId = 155,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 14,
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
                    Kilometer = 13.95,
                    Length = 0.15,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 35,
                    LeftSideElementId = 155,
                    RightSideElementId = null,
                    LineNumber = 1,
                    TrackNumber = "3",
                    Kilometer = 14.1,
                    Length = 0.25,
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
                    Kilometer = 14.1,
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
                    Kilometer = 14.1,
                    Length = 0.25,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 38,
                    LeftSideElementId = 156,
                    RightSideElementId = null,
                    LineNumber = 1,
                    TrackNumber = "4",
                    Kilometer = 14.1,
                    Length = 0.25,
                    MaxUpSpeed = 40,
                    MaxDownSpeed = 80,
                    Gradient = 0,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
            ];

            Dictionary<int, TrackageElement> trackageElementsLookup = trackageElements.ToDictionary(te => te.TrackageElementId, te => te);
            Dictionary<int, Track> tracksLookup = trackageElements.Where(te => te is Track).ToDictionary(te => te.TrackageElementId, te => (Track)te);

            //trackageElementsLookup[1].RightSideElement = trackageElementsLookup[trackageElementsLookup[1].RightSideElementId!.Value];
            //trackageElementsLookup[2].RightSideElement = trackageElementsLookup[trackageElementsLookup[2].RightSideElementId!.Value];
            //trackageElementsLookup[3].RightSideElement = trackageElementsLookup[trackageElementsLookup[3].RightSideElementId!.Value];
            //trackageElementsLookup[6].RightSideElement = trackageElementsLookup[trackageElementsLookup[6].RightSideElementId!.Value];
            //trackageElementsLookup[8].RightSideElement = trackageElementsLookup[trackageElementsLookup[8].RightSideElementId!.Value];
            //trackageElementsLookup[9].RightSideElement = trackageElementsLookup[trackageElementsLookup[9].RightSideElementId!.Value];
            //trackageElementsLookup[11].RightSideElement = trackageElementsLookup[trackageElementsLookup[11].RightSideElementId!.Value];
            //trackageElementsLookup[14].RightSideElement = trackageElementsLookup[trackageElementsLookup[14].RightSideElementId!.Value];
            //trackageElementsLookup[15].RightSideElement = trackageElementsLookup[trackageElementsLookup[15].RightSideElementId!.Value];
            //trackageElementsLookup[16].RightSideElement = trackageElementsLookup[trackageElementsLookup[16].RightSideElementId!.Value];

            //trackageElementsLookup[2].LeftSideElement = trackageElementsLookup[trackageElementsLookup[2].LeftSideElementId!.Value];
            //trackageElementsLookup[3].LeftSideElement = trackageElementsLookup[trackageElementsLookup[3].LeftSideElementId!.Value];
            //trackageElementsLookup[5].LeftSideElement = trackageElementsLookup[trackageElementsLookup[5].LeftSideElementId!.Value];
            //trackageElementsLookup[6].LeftSideElement = trackageElementsLookup[trackageElementsLookup[6].LeftSideElementId!.Value];
            //trackageElementsLookup[8].LeftSideElement = trackageElementsLookup[trackageElementsLookup[8].LeftSideElementId!.Value];
            //trackageElementsLookup[9].LeftSideElement = trackageElementsLookup[trackageElementsLookup[9].LeftSideElementId!.Value];
            //trackageElementsLookup[11].LeftSideElement = trackageElementsLookup[trackageElementsLookup[11].LeftSideElementId!.Value];
            //trackageElementsLookup[14].LeftSideElement = trackageElementsLookup[trackageElementsLookup[14].LeftSideElementId!.Value];
            //trackageElementsLookup[15].LeftSideElement = trackageElementsLookup[trackageElementsLookup[15].LeftSideElementId!.Value];
            //trackageElementsLookup[16].LeftSideElement = trackageElementsLookup[trackageElementsLookup[16].LeftSideElementId!.Value];
            //trackageElementsLookup[17].LeftSideElement = trackageElementsLookup[trackageElementsLookup[17].LeftSideElementId!.Value];

            //Dictionary<int, RailroadSign> signsLookup = [];
            //int signsCounter = 1;
            //trackageElements.ForEach(te =>
            //{
            //    if (te is Track track)
            //    {
            //        signsCounter++;
            //        signsLookup.Add(signsCounter, new RailroadSign()
            //        {
            //            RailroadSignId = signsCounter,
            //            TrackId = te.TrackageElementId,
            //            Track = track,
            //            DistanceFromTrackStart = 0,
            //            IsFacedUp = true,
            //            MaxSpeed = track.MaxUpSpeed
            //        });
            //        signsCounter++;
            //        signsLookup.Add(signsCounter, new RailroadSign()
            //        {
            //            RailroadSignId = signsCounter,
            //            TrackId = te.TrackageElementId,
            //            Track = track,
            //            DistanceFromTrackStart = track.Length,
            //            IsFacedUp = false,
            //            MaxSpeed = track.MaxDownSpeed
            //        });
            //    }
            //});

            //Dictionary<int, RailwaySignal> railwaySignalsLookup = new()
            //{
            //    { 12, new() { RailwaySignalId = 12, TrackId = 2, Track = tracksLookup[2], DistanceFromTrackStart = 0, IsFacedUp = true } },
            //    { 21, new() { RailwaySignalId = 21, TrackId = 1, Track = tracksLookup[1], DistanceFromTrackStart = tracksLookup[1].Length, IsFacedUp = false } },
            //    { 23, new() { RailwaySignalId = 23, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = 0, IsFacedUp = true } },
            //    { 32, new() { RailwaySignalId = 32, TrackId = 2, Track = tracksLookup[2], DistanceFromTrackStart = tracksLookup[2].Length, IsFacedUp = false } },
            //    { 3, new() { RailwaySignalId = 3, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = 2.5, IsFacedUp = true } },
            //    { 33, new() { RailwaySignalId = 33, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = 2.5, IsFacedUp = false } },
            //    { 4, new() { RailwaySignalId = 4, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = tracksLookup[3].Length, IsFacedUp = true } },
            //    { 44, new() { RailwaySignalId = 44, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = tracksLookup[3].Length, IsFacedUp = false } },
            //    { 7, new() { RailwaySignalId = 7, TrackId = 6, Track = tracksLookup[6], DistanceFromTrackStart = tracksLookup[6].Length, IsFacedUp = true } },
            //    { 77, new() { RailwaySignalId = 77, TrackId = 6, Track = tracksLookup[6], DistanceFromTrackStart = tracksLookup[6].Length, IsFacedUp = false } },
            //    { 1516, new() { RailwaySignalId = 1516, TrackId = 16, Track = tracksLookup[16], DistanceFromTrackStart = 0, IsFacedUp = true } },
            //    { 1615, new() { RailwaySignalId = 1615, TrackId = 15, Track = tracksLookup[15], DistanceFromTrackStart = tracksLookup[15].Length, IsFacedUp = false } },
            //    { 1617, new() { RailwaySignalId = 1617, TrackId = 17, Track = tracksLookup[17], DistanceFromTrackStart = 0, IsFacedUp = true } },
            //    { 1716, new() { RailwaySignalId = 1716, TrackId = 16, Track = tracksLookup[16], DistanceFromTrackStart = tracksLookup[16].Length, IsFacedUp = false } },
            //};

            //Dictionary<int, SwitchRoute> switchRoutesLookup = new()
            //{
            //    {1, new() { SwitchRouteId = 1, SwitchId = 4, Switch = (Switch)trackageElementsLookup[4], TrackFromId = 3, TrackFrom = trackageElementsLookup[3], TrackToId = 5, TrackTo = trackageElementsLookup[5] } },
            //    {2, new() { SwitchRouteId = 2, SwitchId = 4, Switch = (Switch)trackageElementsLookup[4], TrackFromId = 3, TrackFrom = trackageElementsLookup[3], TrackToId = 6, TrackTo = trackageElementsLookup[6] } },
            //    {3, new() { SwitchRouteId = 3, SwitchId = 4, Switch = (Switch)trackageElementsLookup[4], TrackFromId = 5, TrackFrom = trackageElementsLookup[5], TrackToId = 3, TrackTo = trackageElementsLookup[3] } },
            //    {4, new() { SwitchRouteId = 4, SwitchId = 4, Switch = (Switch)trackageElementsLookup[4], TrackFromId = 6, TrackFrom = trackageElementsLookup[6], TrackToId = 3, TrackTo = trackageElementsLookup[3] } },
            //    {5, new() { SwitchRouteId = 5, SwitchId = 7, Switch = (Switch)trackageElementsLookup[7], TrackFromId = 6, TrackFrom = trackageElementsLookup[6], TrackToId = 9, TrackTo = trackageElementsLookup[9] } },
            //    {6, new() { SwitchRouteId = 6, SwitchId = 7, Switch = (Switch)trackageElementsLookup[7], TrackFromId = 6, TrackFrom = trackageElementsLookup[6], TrackToId = 8, TrackTo = trackageElementsLookup[8] } },
            //    {7, new() { SwitchRouteId = 7, SwitchId = 7, Switch = (Switch)trackageElementsLookup[7], TrackFromId = 8, TrackFrom = trackageElementsLookup[8], TrackToId = 6, TrackTo = trackageElementsLookup[6] } },
            //    {8, new() { SwitchRouteId = 8, SwitchId = 7, Switch = (Switch)trackageElementsLookup[7], TrackFromId = 9, TrackFrom = trackageElementsLookup[9], TrackToId = 6, TrackTo = trackageElementsLookup[6] } },
            //    {9, new() { SwitchRouteId = 9, SwitchId = 12, Switch = (Switch)trackageElementsLookup[12], TrackFromId = 11, TrackFrom = trackageElementsLookup[11], TrackToId = 14, TrackTo = trackageElementsLookup[14] } },
            //    {10, new() { SwitchRouteId = 10, SwitchId = 12, Switch = (Switch)trackageElementsLookup[12], TrackFromId = 11, TrackFrom = trackageElementsLookup[11], TrackToId = 15, TrackTo = trackageElementsLookup[15] } },
            //    {11, new() { SwitchRouteId = 11, SwitchId = 12, Switch = (Switch)trackageElementsLookup[12], TrackFromId = 14, TrackFrom = trackageElementsLookup[14], TrackToId = 11, TrackTo = trackageElementsLookup[11] } },
            //    {12, new() { SwitchRouteId = 12, SwitchId = 12, Switch = (Switch)trackageElementsLookup[12], TrackFromId = 15, TrackFrom = trackageElementsLookup[15], TrackToId = 11, TrackTo = trackageElementsLookup[11] } },
            //    {13, new() { SwitchRouteId = 13, SwitchId = 10, Switch = (Switch)trackageElementsLookup[10], TrackFromId = 9, TrackFrom = trackageElementsLookup[9], TrackToId = 11, TrackTo = trackageElementsLookup[11] } },
            //    {15, new() { SwitchRouteId = 15, SwitchId = 10, Switch = (Switch)trackageElementsLookup[10], TrackFromId = 11, TrackFrom = trackageElementsLookup[11], TrackToId = 9, TrackTo = trackageElementsLookup[9] } },
            //};

            A.CallTo(() => TrackageElementHolder.GetValues()).Returns(trackageElementsLookup);
            A.CallTo(() => TrackHolder.GetValues()).Returns(tracksLookup);
            //A.CallTo(() => CrossingHolder.GetValues()).Returns([]);
            //A.CallTo(() => RailroadSignHolder.GetValues()).Returns(signsLookup);
            //A.CallTo(() => RailwaySignalHolder.GetValues()).Returns(railwaySignalsLookup);
            //A.CallTo(() => SwitchRouteHolder.GetValues()).Returns(switchRoutesLookup);
            //A.CallTo(() => RegisteredTrainsTracker.GetRegisteredTrain(A<string>.Ignored)).Returns(Train);
        }
    }
}
