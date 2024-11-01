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

namespace EtcsServer.ExtensionMethods
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddSingleton<IHolder<Crossing>, CrossingsHolder>();
            services.AddSingleton<IHolder<RailroadSign>, RailroadSignsHolder>();
            services.AddSingleton<IHolder<RailwaySignal>, RailwaySignalsHolder>();
            services.AddSingleton<IHolder<SwitchRoute>, SwitchRoutesHolder>();
            services.AddSingleton<IHolder<Track>, TracksHolder>();
            services.AddSingleton<IHolder<Train>, TrainsHolder>();

            services.AddSingleton<ITrainPositionTracker, LastKnownPositionsTracker>();
            services.AddSingleton<IRailwaySignalStates, RailwaySignalStates>();
            services.AddSingleton<ISwitchStates, SwitchStates>();
            services.AddSingleton<IRegisteredTrainsTracker, RegisteredTrainsTracker>();

            services.AddSingleton<IRailwaySignalHelper, RailwaySignalHelper>();
            services.AddSingleton<ITrackHelper, TrackHelper>();

            services.AddSingleton<IMovementAuthorityValidator, MovementAuthorityValidator>();
            services.AddSingleton<IMovementAuthorityProvider, MovementAuthorityProvider>();

            return services;
        }
    }
}
