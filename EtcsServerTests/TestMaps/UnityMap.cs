using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryHolders;
using EtcsServer.MapLoading;
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
                LengthMeters = 500,
                MaxSpeed = 200,
                BrakeWeight = 1000
            };
            InitializeHolders();
        }

        protected override void InitializeHolders()
        {
            UnityTrackageMap unityTrackageMap = new();

            Dictionary<int, TrackageElement> trackageElementsLookup = unityTrackageMap.TrackageElementsLookup;

            trackageElementsLookup
                .Where(kvp => kvp.Value.RightSideElementId != null)
                .ToList()
                .ForEach(element => element.Value.RightSideElement = trackageElementsLookup[element.Value.RightSideElementId!.Value]);

            trackageElementsLookup
                .Where(kvp => kvp.Value.LeftSideElementId != null)
                .ToList()
                .ForEach(element => element.Value.LeftSideElement = trackageElementsLookup[element.Value.LeftSideElementId!.Value]);


            Dictionary<int, RailwaySignal> railwaySignalLookup = unityTrackageMap.RailwaySignalsLookup;
            railwaySignalLookup.Add(901, new RailwaySignal() { RailwaySignalId = 901, IsFacedUp = false, TrackId = 211, Track = unityTrackageMap.TracksLookup[211], DistanceFromTrackStart = 1.55 });
            railwaySignalLookup.Add(9011, new RailwaySignal() { RailwaySignalId = 9011, IsFacedUp = false, TrackId = 213, Track = unityTrackageMap.TracksLookup[213], DistanceFromTrackStart = 1.55 });
            railwaySignalLookup.Add(902, new RailwaySignal() { RailwaySignalId = 902, IsFacedUp = true, TrackId = 15, Track = unityTrackageMap.TracksLookup[15], DistanceFromTrackStart = 0.1 });
            railwaySignalLookup.Add(9022, new RailwaySignal() { RailwaySignalId = 9022, IsFacedUp = true, TrackId = 16, Track = unityTrackageMap.TracksLookup[16], DistanceFromTrackStart = 0.15 });

            A.CallTo(() => TrackageElementHolder.GetValues()).Returns(unityTrackageMap.TrackageElementsLookup);
            A.CallTo(() => TrackHolder.GetValues()).Returns(unityTrackageMap.TracksLookup);
            A.CallTo(() => CrossingHolder.GetValues()).Returns(unityTrackageMap.CrossingsLookup);
            A.CallTo(() => CrossingTracksHolder.GetValues()).Returns(unityTrackageMap.CrossingTracksLookup);
            A.CallTo(() => RailroadSignHolder.GetValues()).Returns(unityTrackageMap.RailroadSignsLookup);
            A.CallTo(() => RailwaySignalHolder.GetValues()).Returns(railwaySignalLookup);
            A.CallTo(() => SwitchRouteHolder.GetValues()).Returns(unityTrackageMap.SwitchRoutesLookup);
            A.CallTo(() => RegisteredTrainsTracker.GetRegisteredTrain(A<string>.Ignored)).Returns(Train);
        }
    }
}
