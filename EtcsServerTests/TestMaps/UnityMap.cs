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
        private UnityTrackageMap unityTrackageMap;

        protected override void InitializeHolders()
        {
            unityTrackageMap = new();

            Dictionary<int, TrackageElement> trackageElementsLookup = unityTrackageMap.TrackageElementsLookup;

            trackageElementsLookup
                .Where(kvp => kvp.Value.RightSideElementId != null)
                .ToList()
                .ForEach(element => element.Value.RightSideElement = trackageElementsLookup[element.Value.RightSideElementId!.Value]);

            trackageElementsLookup
                .Where(kvp => kvp.Value.LeftSideElementId != null)
                .ToList()
                .ForEach(element => element.Value.LeftSideElement = trackageElementsLookup[element.Value.LeftSideElementId!.Value]);

            A.CallTo(() => TrackageElementHolder.GetValues()).Returns(unityTrackageMap.TrackageElementsLookup);
            A.CallTo(() => TrackHolder.GetValues()).Returns(unityTrackageMap.TracksLookup);
            A.CallTo(() => CrossingHolder.GetValues()).Returns(unityTrackageMap.CrossingsLookup);
            A.CallTo(() => CrossingTracksHolder.GetValues()).Returns(unityTrackageMap.CrossingTracksLookup);
            A.CallTo(() => RailroadSignHolder.GetValues()).Returns(unityTrackageMap.RailroadSignsLookup);
            A.CallTo(() => RailwaySignalHolder.GetValues()).Returns(unityTrackageMap.RailwaySignalsLookup);
            A.CallTo(() => SwitchRouteHolder.GetValues()).Returns(unityTrackageMap.SwitchRoutesLookup);
            A.CallTo(() => SwitchDirectionHolder.GetValues()).Returns(unityTrackageMap.SwitchDirections);
        }
    }
}
