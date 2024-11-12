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
    class TestMap1 : MockTestMap
    {
        public TrainDto Train { get; set; }

        public TestMap1()
        {
            this.Train = new()
            {
                TrainId = "123",
                LengthMeters = 800,
                MaxSpeed = 200,
                BrakeWeight = 500
            };
            InitializeHolders();
        }

        protected override void InitializeHolders()
        {
            List<TrackageElement> trackageElements = [
                new Track() {
                    TrackageElementId = 1,
                    LeftSideElementId = null,
                    RightSideElementId = 2,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 0,
                    Length = 2,
                    MaxUpSpeed = 200,
                    MaxDownSpeed = 200,
                    Gradient = 20,
                    TrackPosition = TrackPosition.OUTSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 2,
                    LeftSideElementId = 1,
                    RightSideElementId = 3,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 2,
                    Length = 1,
                    MaxUpSpeed = 180,
                    MaxDownSpeed = 180,
                    Gradient = 18,
                    TrackPosition = TrackPosition.INCOMING_ZONE
                },
                new Track() {
                    TrackageElementId = 3,
                    LeftSideElementId = 2,
                    RightSideElementId = 4,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 3,
                    Length = 5,
                    MaxUpSpeed = 70,
                    MaxDownSpeed = 70,
                    Gradient = 7,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Switch() {
                    TrackageElementId = 4,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Track() {
                    TrackageElementId = 5,
                    LeftSideElementId = 4,
                    RightSideElementId = null,
                    LineNumber = 1,
                    TrackNumber = "1",
                    Kilometer = 8,
                    Length = 1,
                    MaxUpSpeed = 25,
                    MaxDownSpeed = 25,
                    Gradient = 2.5,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 6,
                    LeftSideElementId = 4,
                    RightSideElementId = 7,
                    LineNumber = 2,
                    TrackNumber = "1",
                    Kilometer = 0,
                    Length = 1,
                    MaxUpSpeed = 30,
                    MaxDownSpeed = 30,
                    Gradient = 3,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Switch() {
                    TrackageElementId = 7,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Track() {
                    TrackageElementId = 8,
                    LeftSideElementId = 7,
                    RightSideElementId = 13,
                    LineNumber = 2,
                    TrackNumber = "1",
                    Kilometer = 1,
                    Length = 1.5,
                    MaxUpSpeed = 90,
                    MaxDownSpeed = 90,
                    Gradient = 9,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new SwitchingTrack() {
                    TrackageElementId = 9,
                    LeftSideElementId = 7,
                    RightSideElementId = 10,
                    Length = 0.3,
                    MaxSpeed = 20,
                    Gradient = 2,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Switch() {
                    TrackageElementId = 10,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Track() {
                    TrackageElementId = 11,
                    LeftSideElementId = 10,
                    RightSideElementId = 12,
                    LineNumber = 2,
                    TrackNumber = "2",
                    Kilometer = 1.25,
                    Length = 1,
                    MaxUpSpeed = 50,
                    MaxDownSpeed = 50,
                    Gradient = 5,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Switch() {
                    TrackageElementId = 12,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new Switch() {
                    TrackageElementId = 13,
                    LeftSideElementId = null,
                    RightSideElementId = null
                },
                new SwitchingTrack() {
                    TrackageElementId = 14,
                    LeftSideElementId = 12,
                    RightSideElementId = 13,
                    Length = 0.3,
                    MaxSpeed = 30,
                    Gradient = 3,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 15,
                    LeftSideElementId = 12,
                    RightSideElementId = 16,
                    LineNumber = 2,
                    TrackNumber = "2",
                    Kilometer = 2.25,
                    Length = 0.75,
                    MaxUpSpeed = 65,
                    MaxDownSpeed = 65,
                    Gradient = 6.5,
                    TrackPosition = TrackPosition.INSIDE_ZONE
                },
                new Track() {
                    TrackageElementId = 16,
                    LeftSideElementId = 15,
                    RightSideElementId = 17,
                    LineNumber = 2,
                    TrackNumber = "2",
                    Kilometer = 3,
                    Length = 1,
                    MaxUpSpeed = 80,
                    MaxDownSpeed = 80,
                    Gradient = 8,
                    TrackPosition = TrackPosition.INCOMING_ZONE
                },
                new Track() {
                    TrackageElementId = 17,
                    LeftSideElementId = 16,
                    RightSideElementId = null,
                    LineNumber = 2,
                    TrackNumber = "2",
                    Kilometer = 5,
                    Length = 2,
                    MaxUpSpeed = 140,
                    MaxDownSpeed = 140,
                    Gradient = 14,
                    TrackPosition = TrackPosition.OUTSIDE_ZONE
                },
            ];
            Dictionary<int, TrackageElement> trackageElementsLookup = trackageElements.ToDictionary(te => te.TrackageElementId, te => te);
            Dictionary<int, Track> tracksLookup = trackageElements.Where(te => te is Track).ToDictionary(te => te.TrackageElementId, te => (Track)te);

            trackageElementsLookup[1].RightSideElement = trackageElementsLookup[trackageElementsLookup[1].RightSideElementId!.Value];
            trackageElementsLookup[2].RightSideElement = trackageElementsLookup[trackageElementsLookup[2].RightSideElementId!.Value];
            trackageElementsLookup[3].RightSideElement = trackageElementsLookup[trackageElementsLookup[3].RightSideElementId!.Value];
            trackageElementsLookup[6].RightSideElement = trackageElementsLookup[trackageElementsLookup[6].RightSideElementId!.Value];
            trackageElementsLookup[8].RightSideElement = trackageElementsLookup[trackageElementsLookup[8].RightSideElementId!.Value];
            trackageElementsLookup[9].RightSideElement = trackageElementsLookup[trackageElementsLookup[9].RightSideElementId!.Value];
            trackageElementsLookup[11].RightSideElement = trackageElementsLookup[trackageElementsLookup[11].RightSideElementId!.Value];
            trackageElementsLookup[14].RightSideElement = trackageElementsLookup[trackageElementsLookup[14].RightSideElementId!.Value];
            trackageElementsLookup[15].RightSideElement = trackageElementsLookup[trackageElementsLookup[15].RightSideElementId!.Value];
            trackageElementsLookup[16].RightSideElement = trackageElementsLookup[trackageElementsLookup[16].RightSideElementId!.Value];

            trackageElementsLookup[2].LeftSideElement = trackageElementsLookup[trackageElementsLookup[2].LeftSideElementId!.Value];
            trackageElementsLookup[3].LeftSideElement = trackageElementsLookup[trackageElementsLookup[3].LeftSideElementId!.Value];
            trackageElementsLookup[5].LeftSideElement = trackageElementsLookup[trackageElementsLookup[5].LeftSideElementId!.Value];
            trackageElementsLookup[6].LeftSideElement = trackageElementsLookup[trackageElementsLookup[6].LeftSideElementId!.Value];
            trackageElementsLookup[8].LeftSideElement = trackageElementsLookup[trackageElementsLookup[8].LeftSideElementId!.Value];
            trackageElementsLookup[9].LeftSideElement = trackageElementsLookup[trackageElementsLookup[9].LeftSideElementId!.Value];
            trackageElementsLookup[11].LeftSideElement = trackageElementsLookup[trackageElementsLookup[11].LeftSideElementId!.Value];
            trackageElementsLookup[14].LeftSideElement = trackageElementsLookup[trackageElementsLookup[14].LeftSideElementId!.Value];
            trackageElementsLookup[15].LeftSideElement = trackageElementsLookup[trackageElementsLookup[15].LeftSideElementId!.Value];
            trackageElementsLookup[16].LeftSideElement = trackageElementsLookup[trackageElementsLookup[16].LeftSideElementId!.Value];
            trackageElementsLookup[17].LeftSideElement = trackageElementsLookup[trackageElementsLookup[17].LeftSideElementId!.Value];

            Dictionary<int, RailroadSign> signsLookup = [];
            int signsCounter = 1;
            trackageElements.ForEach(te =>
            {
                if (te is Track track)
                {
                    signsCounter++;
                    signsLookup.Add(signsCounter, new RailroadSign()
                    {
                        RailroadSignId = signsCounter,
                        TrackId = te.TrackageElementId,
                        Track = track,
                        DistanceFromTrackStart = 0,
                        IsFacedUp = true,
                        MaxSpeed = track.MaxUpSpeed
                    });
                    signsCounter++;
                    signsLookup.Add(signsCounter, new RailroadSign()
                    {
                        RailroadSignId = signsCounter,
                        TrackId = te.TrackageElementId,
                        Track = track,
                        DistanceFromTrackStart = track.Length,
                        IsFacedUp = false,
                        MaxSpeed = track.MaxDownSpeed
                    });
                }
            });

            Dictionary<int, RailwaySignal> railwaySignalsLookup = new()
            {
                { 12, new() { RailwaySignalId = 12, TrackId = 2, Track = tracksLookup[2], DistanceFromTrackStart = 0, IsFacedUp = true } },
                { 21, new() { RailwaySignalId = 21, TrackId = 1, Track = tracksLookup[1], DistanceFromTrackStart = tracksLookup[1].Length, IsFacedUp = false } },
                { 23, new() { RailwaySignalId = 23, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = 0, IsFacedUp = true } },
                { 32, new() { RailwaySignalId = 32, TrackId = 2, Track = tracksLookup[2], DistanceFromTrackStart = tracksLookup[2].Length, IsFacedUp = false } },
                { 3, new() { RailwaySignalId = 3, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = 2.5, IsFacedUp = true } },
                { 33, new() { RailwaySignalId = 33, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = 2.5, IsFacedUp = false } },
                { 4, new() { RailwaySignalId = 4, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = tracksLookup[3].Length, IsFacedUp = true } },
                { 44, new() { RailwaySignalId = 44, TrackId = 3, Track = tracksLookup[3], DistanceFromTrackStart = tracksLookup[3].Length, IsFacedUp = false } },
                { 7, new() { RailwaySignalId = 7, TrackId = 6, Track = tracksLookup[6], DistanceFromTrackStart = tracksLookup[6].Length, IsFacedUp = true } },
                { 77, new() { RailwaySignalId = 77, TrackId = 6, Track = tracksLookup[6], DistanceFromTrackStart = tracksLookup[6].Length, IsFacedUp = false } },
                { 1516, new() { RailwaySignalId = 1516, TrackId = 16, Track = tracksLookup[16], DistanceFromTrackStart = 0, IsFacedUp = true } },
                { 1615, new() { RailwaySignalId = 1615, TrackId = 15, Track = tracksLookup[15], DistanceFromTrackStart = tracksLookup[15].Length, IsFacedUp = false } },
                { 1617, new() { RailwaySignalId = 1617, TrackId = 17, Track = tracksLookup[17], DistanceFromTrackStart = 0, IsFacedUp = true } },
                { 1716, new() { RailwaySignalId = 1716, TrackId = 16, Track = tracksLookup[16], DistanceFromTrackStart = tracksLookup[16].Length, IsFacedUp = false } },
            };

            Dictionary<int, SwitchRoute> switchRoutesLookup = new()
            {
                {1, new() { SwitchRouteId = 1, SwitchId = 4, Switch = (Switch)trackageElementsLookup[4], TrackFromId = 3, TrackFrom = trackageElementsLookup[3], TrackToId = 5, TrackTo = trackageElementsLookup[5] } },
                {2, new() { SwitchRouteId = 2, SwitchId = 4, Switch = (Switch)trackageElementsLookup[4], TrackFromId = 3, TrackFrom = trackageElementsLookup[3], TrackToId = 6, TrackTo = trackageElementsLookup[6] } },
                {3, new() { SwitchRouteId = 3, SwitchId = 4, Switch = (Switch)trackageElementsLookup[4], TrackFromId = 5, TrackFrom = trackageElementsLookup[5], TrackToId = 3, TrackTo = trackageElementsLookup[3] } },
                {4, new() { SwitchRouteId = 4, SwitchId = 4, Switch = (Switch)trackageElementsLookup[4], TrackFromId = 6, TrackFrom = trackageElementsLookup[6], TrackToId = 3, TrackTo = trackageElementsLookup[3] } },
                {5, new() { SwitchRouteId = 5, SwitchId = 7, Switch = (Switch)trackageElementsLookup[7], TrackFromId = 6, TrackFrom = trackageElementsLookup[6], TrackToId = 9, TrackTo = trackageElementsLookup[9] } },
                {6, new() { SwitchRouteId = 6, SwitchId = 7, Switch = (Switch)trackageElementsLookup[7], TrackFromId = 6, TrackFrom = trackageElementsLookup[6], TrackToId = 8, TrackTo = trackageElementsLookup[8] } },
                {7, new() { SwitchRouteId = 7, SwitchId = 7, Switch = (Switch)trackageElementsLookup[7], TrackFromId = 8, TrackFrom = trackageElementsLookup[8], TrackToId = 6, TrackTo = trackageElementsLookup[6] } },
                {8, new() { SwitchRouteId = 8, SwitchId = 7, Switch = (Switch)trackageElementsLookup[7], TrackFromId = 9, TrackFrom = trackageElementsLookup[9], TrackToId = 6, TrackTo = trackageElementsLookup[6] } },
                {9, new() { SwitchRouteId = 9, SwitchId = 12, Switch = (Switch)trackageElementsLookup[12], TrackFromId = 11, TrackFrom = trackageElementsLookup[11], TrackToId = 14, TrackTo = trackageElementsLookup[14] } },
                {10, new() { SwitchRouteId = 10, SwitchId = 12, Switch = (Switch)trackageElementsLookup[12], TrackFromId = 11, TrackFrom = trackageElementsLookup[11], TrackToId = 15, TrackTo = trackageElementsLookup[15] } },
                {11, new() { SwitchRouteId = 11, SwitchId = 12, Switch = (Switch)trackageElementsLookup[12], TrackFromId = 14, TrackFrom = trackageElementsLookup[14], TrackToId = 11, TrackTo = trackageElementsLookup[11] } },
                {12, new() { SwitchRouteId = 12, SwitchId = 12, Switch = (Switch)trackageElementsLookup[12], TrackFromId = 15, TrackFrom = trackageElementsLookup[15], TrackToId = 11, TrackTo = trackageElementsLookup[11] } },
                {13, new() { SwitchRouteId = 13, SwitchId = 10, Switch = (Switch)trackageElementsLookup[10], TrackFromId = 9, TrackFrom = trackageElementsLookup[9], TrackToId = 11, TrackTo = trackageElementsLookup[11] } },
                {15, new() { SwitchRouteId = 15, SwitchId = 10, Switch = (Switch)trackageElementsLookup[10], TrackFromId = 11, TrackFrom = trackageElementsLookup[11], TrackToId = 9, TrackTo = trackageElementsLookup[9] } },
            };

            A.CallTo(() => TrackageElementHolder.GetValues()).Returns(trackageElementsLookup);
            A.CallTo(() => TrackHolder.GetValues()).Returns(tracksLookup);
            A.CallTo(() => CrossingHolder.GetValues()).Returns([]);
            A.CallTo(() => CrossingTracksHolder.GetValues()).Returns([]);
            A.CallTo(() => RailroadSignHolder.GetValues()).Returns(signsLookup);
            A.CallTo(() => RailwaySignalHolder.GetValues()).Returns(railwaySignalsLookup);
            A.CallTo(() => SwitchRouteHolder.GetValues()).Returns(switchRoutesLookup);
            A.CallTo(() => RegisteredTrainsTracker.GetRegisteredTrain(A<string>.Ignored)).Returns(Train);
        }
    }
}
