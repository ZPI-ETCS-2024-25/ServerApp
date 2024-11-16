using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DecisionMakers;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.DriverDataCollectors;
using EtcsServer.ExtensionMethods;
using EtcsServer.Helpers.Contract;
using EtcsServer.Helpers;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using FakeItEasy;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using EtcsServer.DriverAppDto;
using EtcsServer.Controllers;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using EtcsServerTests.Helpers;

namespace EtcsServerTests.TestMaps
{
    public abstract class MockTestMap : ITestMap
    {
        public IHolder<Crossing> CrossingHolder { get; set; }
        public IHolder<CrossingTrack> CrossingTracksHolder { get; set; }
        public IHolder<RailroadSign> RailroadSignHolder { get; set; }
        public IHolder<RailwaySignal> RailwaySignalHolder { get; set; }
        public IHolder<SwitchRoute> SwitchRouteHolder { get; set; }
        public IHolder<Track> TrackHolder { get; set; }
        public IHolder<TrackageElement> TrackageElementHolder { get; set; }
        public IRegisteredTrainsTracker RegisteredTrainsTracker { get; set; }
        public ITrainPositionTracker TrainPositionTracker { get; set; }
        public IRailwaySignalStates RailwaySignalStates { get; set; }
        public ISwitchStates SwitchStates { get; set; }

        protected abstract void InitializeHolders();

        protected MockTestMap()
        {
            TrackHolder = A.Fake<IHolder<Track>>();
            TrackageElementHolder = A.Fake<IHolder<TrackageElement>>();
            CrossingHolder = A.Fake<IHolder<Crossing>>();
            CrossingTracksHolder = A.Fake<IHolder<CrossingTrack>>();
            RailroadSignHolder = A.Fake<IHolder<RailroadSign>>();
            RailwaySignalHolder = A.Fake<IHolder<RailwaySignal>>();
            SwitchRouteHolder = A.Fake<IHolder<SwitchRoute>>();
            RegisteredTrainsTracker = A.Fake<IRegisteredTrainsTracker>();
            TrainPositionTracker = A.Fake<ITrainPositionTracker>();
            RailwaySignalStates = A.Fake<IRailwaySignalStates>();
            SwitchStates = A.Fake<ISwitchStates>();
        }

        public ServiceProvider GetTestMapServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });

            serviceCollection.AddProjectServices(new TestConfiguration().Configuration);

            serviceCollection.AddSingleton<IHolder<Crossing>>(CrossingHolder);
            serviceCollection.AddSingleton<IHolder<CrossingTrack>>(CrossingTracksHolder);
            serviceCollection.AddSingleton<IHolder<RailroadSign>>(RailroadSignHolder);
            serviceCollection.AddSingleton<IHolder<RailwaySignal>>(RailwaySignalHolder);
            serviceCollection.AddSingleton<IHolder<SwitchRoute>>(SwitchRouteHolder);
            serviceCollection.AddSingleton<IHolder<Track>>(TrackHolder);
            serviceCollection.AddSingleton<IHolder<TrackageElement>>(TrackageElementHolder);

            serviceCollection.AddSingleton<ITrainPositionTracker>(TrainPositionTracker);
            serviceCollection.AddSingleton<IRailwaySignalStates>(RailwaySignalStates);
            serviceCollection.AddSingleton<ISwitchStates>(SwitchStates);
            serviceCollection.AddSingleton<IRegisteredTrainsTracker>(RegisteredTrainsTracker);

            serviceCollection.AddScoped<DriverAppController>();
            serviceCollection.AddScoped<UnityAppController>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
