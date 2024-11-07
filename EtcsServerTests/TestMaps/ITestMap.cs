using EtcsServer.Database.Entity;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtcsServerTests.TestMaps
{
    public interface ITestMap
    {
        IHolder<Crossing> CrossingHolder { get; set; }
        IHolder<RailroadSign> RailroadSignHolder { get; set; }
        IHolder<RailwaySignal> RailwaySignalHolder { get; set; }
        IHolder<SwitchRoute> SwitchRouteHolder { get; set; }
        IHolder<Track> TrackHolder { get; set; }
        IHolder<TrackageElement> TrackageElementHolder { get; set; }
        IRegisteredTrainsTracker RegisteredTrainsTracker { get; set; }
    }
}
