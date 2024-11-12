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
                LengthMeters = 600,
                MaxSpeed = 200,
                BrakeWeight = 1000
            };
            InitializeHolders();
        }

        protected override void InitializeHolders()
        {

            double switchingTrackLength = 0.05;
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
                    RightSideElementId = 112,
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
                    LeftSideElementId = 234,
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
                    LeftSideElementId = 233,
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
                    RightSideElementId = 227,
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

            Dictionary<int, TrackageElement> trackageElementsLookup = trackageElements.ToDictionary(te => te.TrackageElementId, te => te);
            Dictionary<int, Track> tracksLookup = trackageElements.Where(te => te is Track).ToDictionary(te => te.TrackageElementId, te => (Track)te);

            trackageElementsLookup
                .Where(kvp => kvp.Value.RightSideElementId != null)
                .ToList()
                .ForEach(element => element.Value.RightSideElement = trackageElementsLookup[element.Value.RightSideElementId!.Value]);

            trackageElementsLookup
                .Where(kvp => kvp.Value.LeftSideElementId != null)
                .ToList()
                .ForEach(element => element.Value.LeftSideElement = trackageElementsLookup[element.Value.LeftSideElementId!.Value]);

            Dictionary<int, RailroadSign> signsLookup = [];

            Dictionary<int, RailwaySignal> railwaySignalsLookup = new()
            {
                { 1, new() { RailwaySignalId = 1, TrackId = 1, Track = tracksLookup[1], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 2, new() { RailwaySignalId = 2, TrackId = 2, Track = tracksLookup[2], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 4, new() { RailwaySignalId = 4, TrackId = 4, Track = tracksLookup[4], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 11, new() { RailwaySignalId = 11, TrackId = 1, Track = tracksLookup[1], DistanceFromTrackStart = 0.15, IsFacedUp = true } },
                { 22, new() { RailwaySignalId = 22, TrackId = 2, Track = tracksLookup[2], DistanceFromTrackStart = 0.15, IsFacedUp = true } },
                { 44, new() { RailwaySignalId = 44, TrackId = 4, Track = tracksLookup[4], DistanceFromTrackStart = 0.15, IsFacedUp = true } },
                { 9209, new() { RailwaySignalId = 9209, TrackId = 9, Track = tracksLookup[9], DistanceFromTrackStart = tracksLookup[9].Length, IsFacedUp = false } },
                { 10210, new() { RailwaySignalId = 10210, TrackId = 10, Track = tracksLookup[10], DistanceFromTrackStart = tracksLookup[10].Length, IsFacedUp = false } },
                { 213, new() { RailwaySignalId = 213, TrackId = 213, Track = tracksLookup[213], DistanceFromTrackStart = 1.55, IsFacedUp = true } },
                { 211, new() { RailwaySignalId = 211, TrackId = 211, Track = tracksLookup[211], DistanceFromTrackStart = 1.55, IsFacedUp = true } },
                { 15, new() { RailwaySignalId = 15, TrackId = 15, Track = tracksLookup[15], DistanceFromTrackStart = 0.1, IsFacedUp = false } },
                { 16, new() { RailwaySignalId = 16, TrackId = 16, Track = tracksLookup[16], DistanceFromTrackStart = 0.15, IsFacedUp = false } },
                { 1515, new() { RailwaySignalId = 1515, TrackId = 15, Track = tracksLookup[15], DistanceFromTrackStart = 5.298, IsFacedUp = true } },
                { 1616, new() { RailwaySignalId = 1616, TrackId = 16, Track = tracksLookup[16], DistanceFromTrackStart = 5.348, IsFacedUp = true } },
                { 22222, new() { RailwaySignalId = 22222, TrackId = 22, Track = tracksLookup[22], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 21221, new() { RailwaySignalId = 21221, TrackId = 21, Track = tracksLookup[21], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 227229, new() { RailwaySignalId = 227229, TrackId = 227, Track = tracksLookup[227], DistanceFromTrackStart = tracksLookup[227].Length, IsFacedUp = true } },
                { 228230, new() { RailwaySignalId = 228230, TrackId = 228, Track = tracksLookup[228], DistanceFromTrackStart = tracksLookup[228].Length, IsFacedUp = true } },
                { 28, new() { RailwaySignalId = 28, TrackId = 28, Track = tracksLookup[28], DistanceFromTrackStart = 0.15, IsFacedUp = false } },
                { 27, new() { RailwaySignalId = 27, TrackId = 27, Track = tracksLookup[27], DistanceFromTrackStart = 0.1, IsFacedUp = false } },
                { 28238, new() { RailwaySignalId = 28238, TrackId = 28, Track = tracksLookup[28], DistanceFromTrackStart = tracksLookup[28].Length, IsFacedUp = false } },
                { 27237, new() { RailwaySignalId = 27237, TrackId = 27, Track = tracksLookup[27], DistanceFromTrackStart = tracksLookup[27].Length, IsFacedUp = false } },
                { 3838, new() { RailwaySignalId = 3838, TrackId = 38, Track = tracksLookup[38], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 3737, new() { RailwaySignalId = 3737, TrackId = 37, Track = tracksLookup[37], DistanceFromTrackStart = 0.05, IsFacedUp = false } },
                { 3636, new() { RailwaySignalId = 3636, TrackId = 36, Track = tracksLookup[36], DistanceFromTrackStart = 0.05, IsFacedUp = false } },
                { 3535, new() { RailwaySignalId = 3535, TrackId = 35, Track = tracksLookup[35], DistanceFromTrackStart = 0, IsFacedUp = false } },
                { 38, new() { RailwaySignalId = 38, TrackId = 38, Track = tracksLookup[38], DistanceFromTrackStart = tracksLookup[38].Length, IsFacedUp = true } },
                { 37, new() { RailwaySignalId = 37, TrackId = 37, Track = tracksLookup[37], DistanceFromTrackStart = tracksLookup[37].Length, IsFacedUp = true } },
                { 36, new() { RailwaySignalId = 36, TrackId = 36, Track = tracksLookup[36], DistanceFromTrackStart = tracksLookup[36].Length, IsFacedUp = true } },
                { 35, new() { RailwaySignalId = 35, TrackId = 35, Track = tracksLookup[35], DistanceFromTrackStart = tracksLookup[35].Length, IsFacedUp = true } },
            };

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
