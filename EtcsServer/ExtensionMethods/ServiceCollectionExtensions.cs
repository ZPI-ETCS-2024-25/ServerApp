using EtcsServer.Database.Entity;
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
using EtcsServer.Senders.Contracts;
using EtcsServer.Senders;

namespace EtcsServer.ExtensionMethods
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServerProperties>(configuration.GetSection("ServerProperties"));
            services.Configure<SecurityConfiguration>(configuration.GetSection("Security"));
            services.Configure<EtcsProperties>(configuration.GetSection("EtcsProperties"));

            services.AddSingleton<IHolder<Crossing>, CrossingsHolder>();
            services.AddSingleton<IHolder<CrossingTrack>, CrossingTracksHolder>();
            services.AddSingleton<IHolder<RailroadSign>, RailroadSignsHolder>();
            services.AddSingleton<IHolder<RailwaySignal>, RailwaySignalsHolder>();
            services.AddSingleton<IHolder<SwitchRoute>, SwitchRoutesHolder>();
            services.AddSingleton<IHolder<Track>, TracksHolder>();
            services.AddSingleton<IHolder<TrackageElement>, TrackageElementHolder>();
            services.AddSingleton<IHolder<Train>, TrainsHolder>();
            services.AddSingleton<IHolder<SwitchDirection>, SwitchDirectionHolder>();

            services.AddSingleton<ITrainPositionTracker, LastKnownPositionsTracker>();
            services.AddSingleton<IRailwaySignalStates, RailwaySignalStates>();
            services.AddSingleton<ISwitchStates, SwitchStates>();
            services.AddSingleton<ICrossingStates, CrossingStates>();
            services.AddSingleton<IRegisteredTrainsTracker, RegisteredTrainsTracker>();
            services.AddSingleton<ISwitchDirectionStates, SwitchDirectionStates>();

            services.AddSingleton<IRailwaySignalHelper, RailwaySignalHelper>();
            services.AddSingleton<ITrackHelper, TrackHelper>();

            services.AddSingleton<IMovementAuthorityValidator, MovementAuthorityValidator>();
            services.AddSingleton<IMovementAuthorityProvider, MovementAuthorityProvider>();
            services.AddSingleton<IMovementAuthorityTracker, MovementAuthorityTracker>();

            services.AddSingleton<ISecurityManager, SecurityManager>();

            services.AddSingleton<IDriverAppSender, DriverAppSender>();

            return services;
        }
    }
}
