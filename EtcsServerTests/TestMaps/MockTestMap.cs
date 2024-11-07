using EtcsServer.Database.Entity;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtcsServerTests.TestMaps
{
    public abstract class MockTestMap : ITestMap
    {
        public IHolder<Crossing> CrossingHolder { get; set; }
        public IHolder<RailroadSign> RailroadSignHolder { get; set; }
        public IHolder<RailwaySignal> RailwaySignalHolder { get; set; }
        public IHolder<SwitchRoute> SwitchRouteHolder { get; set; }
        public IHolder<Track> TrackHolder { get; set; }
        public IHolder<TrackageElement> TrackageElementHolder { get; set; }
        public IRegisteredTrainsTracker RegisteredTrainsTracker { get; set; }

        protected MockTestMap()
        {
            TrackHolder = A.Fake<IHolder<Track>>();
            TrackageElementHolder = A.Fake<IHolder<TrackageElement>>();
            CrossingHolder = A.Fake<IHolder<Crossing>>();
            RailroadSignHolder = A.Fake<IHolder<RailroadSign>>();
            RailwaySignalHolder = A.Fake<IHolder<RailwaySignal>>();
            SwitchRouteHolder = A.Fake<IHolder<SwitchRoute>>();
            RegisteredTrainsTracker = A.Fake<IRegisteredTrainsTracker>();
        }
    }
}
