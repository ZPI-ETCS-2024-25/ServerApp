
using EtcsServer.Configuration;
using EtcsServer.Database;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverDataCollectors;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.Helpers;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using Microsoft.EntityFrameworkCore;

namespace EtcsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("EtcsDbConnectionString");
            builder.Services.AddDbContext<EtcsDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });


            builder.Services.AddSingleton<IHolder<Crossing>, CrossingsHolder>();
            builder.Services.AddSingleton<IHolder<RailroadSign>, RailroadSignsHolder>();
            builder.Services.AddSingleton<IHolder<RailwaySignal>, RailwaySignalsHolder>();
            builder.Services.AddSingleton<IHolder<SwitchRoute>, SwitchRoutesHolder>();
            builder.Services.AddSingleton<IHolder<Track>, TracksHolder>();
            builder.Services.AddSingleton<IHolder<Train>, TrainsHolder>();

            builder.Services.AddSingleton<ITrainPositionTracker, LastKnownPositionsTracker>();
            builder.Services.AddSingleton<IRailwaySignalStates, RailwaySignalStates>();
            builder.Services.AddSingleton<ISwitchStates, SwitchStates>();
            builder.Services.AddSingleton<IRegisteredTrainsTracker, RegisteredTrainsTracker>();

            builder.Services.AddSingleton<IRailwaySignalHelper, RailwaySignalHelper>();
            builder.Services.AddSingleton<ITrackHelper, TrackHelper>();

            builder.Services.AddSingleton<IMovementAuthorityValidator, MovementAuthorityValidator>();
            builder.Services.AddSingleton<IMovementAuthorityProvider, MovementAuthorityProvider>();

            builder.Services.Configure<ServerProperties>(builder.Configuration.GetSection("ServerProperties"));
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IHolder<Crossing>>();
                scope.ServiceProvider.GetRequiredService<IHolder<RailroadSign>>();
                scope.ServiceProvider.GetRequiredService<IHolder<RailwaySignal>>();
                scope.ServiceProvider.GetRequiredService<IHolder<SwitchRoute>>();
                scope.ServiceProvider.GetRequiredService<IHolder<Track>>();
                scope.ServiceProvider.GetRequiredService<IHolder<Train>>();

                scope.ServiceProvider.GetRequiredService<ITrainPositionTracker>();
                scope.ServiceProvider.GetRequiredService<IRailwaySignalStates>();
                scope.ServiceProvider.GetRequiredService<ISwitchStates>();
                scope.ServiceProvider.GetRequiredService<IRegisteredTrainsTracker>();

                scope.ServiceProvider.GetRequiredService<IRailwaySignalHelper>();
                scope.ServiceProvider.GetRequiredService<ITrackHelper>();

                scope.ServiceProvider.GetRequiredService<IMovementAuthorityValidator>();
                scope.ServiceProvider.GetRequiredService<IMovementAuthorityProvider>();

            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
