﻿using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DecisionMakers;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.DriverDataCollectors;
using EtcsServer.Helpers.Contract;
using EtcsServer.Helpers;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;
using EtcsServer.Security;
using EtcsServer.Configuration;

namespace EtcsServer.ExtensionMethods
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServerProperties>(configuration.GetSection("ServerProperties"));
            services.Configure<SecurityConfiguration>(configuration.GetSection("Security"));

            services.AddSingleton<IHolder<Crossing>, CrossingsHolder>();
            services.AddSingleton<IHolder<RailroadSign>, RailroadSignsHolder>();
            services.AddSingleton<IHolder<RailwaySignal>, RailwaySignalsHolder>();
            services.AddSingleton<IHolder<SwitchRoute>, SwitchRoutesHolder>();
            services.AddSingleton<IHolder<Track>, TracksHolder>();
            services.AddSingleton<IHolder<TrackageElement>, TrackageElementHolder>();
            services.AddSingleton<IHolder<Train>, TrainsHolder>();

            services.AddSingleton<ITrainPositionTracker, LastKnownPositionsTracker>();
            services.AddSingleton<IRailwaySignalStates, RailwaySignalStates>();
            services.AddSingleton<ISwitchStates, SwitchStates>();
            services.AddSingleton<IRegisteredTrainsTracker, RegisteredTrainsTracker>();

            services.AddSingleton<IRailwaySignalHelper, RailwaySignalHelper>();
            services.AddSingleton<ITrackHelper, TrackHelper>();

            services.AddSingleton<IMovementAuthorityValidator, MovementAuthorityValidator>();
            services.AddSingleton<IMovementAuthorityProvider, MovementAuthorityProvider>();

            services.AddSingleton<ISecurityManager, SecurityManager>();

            return services;
        }
    }
}
