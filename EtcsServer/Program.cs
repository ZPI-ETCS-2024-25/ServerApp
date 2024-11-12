
using EtcsServer.Configuration;
using EtcsServer.Database;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverDataCollectors;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.ExtensionMethods;
using EtcsServer.Helpers;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using EtcsServer.MapLoading;
using EtcsServer.Security;
using Microsoft.EntityFrameworkCore;

namespace EtcsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //var connectionString = builder.Configuration.GetConnectionString("EtcsDbConnectionString");
            //builder.Services.AddDbContext<EtcsDbContext>(options =>
            //{
            //    options.UseSqlServer(connectionString);
            //});
            builder.Services.AddDbContext<EtcsDbContext>(options =>
            {
                options.UseInMemoryDatabase("EtcsInMemoryDatabase");
            });

            builder.Services.AddProjectServices(builder.Configuration);
            builder.Services.AddControllers();

            //register map
            builder.Services.AddSingleton<ITrackageMap, UnityTrackageMap>();
            builder.Services.AddSingleton<IMapLoader, MapLoader>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //map loading
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EtcsDbContext>();
                var mapLoader = scope.ServiceProvider.GetRequiredService<IMapLoader>();
                var trackageMap = scope.ServiceProvider.GetRequiredService<ITrackageMap>();
                mapLoader.LoadMapIntoDatabase(dbContext, trackageMap);
            }

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IHolder<Crossing>>();
                scope.ServiceProvider.GetRequiredService<IHolder<RailroadSign>>();
                scope.ServiceProvider.GetRequiredService<IHolder<RailwaySignal>>();
                scope.ServiceProvider.GetRequiredService<IHolder<SwitchRoute>>();
                scope.ServiceProvider.GetRequiredService<IHolder<Track>>();
                scope.ServiceProvider.GetRequiredService<IHolder<TrackageElement>>();
                scope.ServiceProvider.GetRequiredService<IHolder<Train>>();

                scope.ServiceProvider.GetRequiredService<ITrainPositionTracker>();
                scope.ServiceProvider.GetRequiredService<IRailwaySignalStates>();
                scope.ServiceProvider.GetRequiredService<ISwitchStates>();
                scope.ServiceProvider.GetRequiredService<IRegisteredTrainsTracker>();

                scope.ServiceProvider.GetRequiredService<IRailwaySignalHelper>();
                scope.ServiceProvider.GetRequiredService<ITrackHelper>();

                scope.ServiceProvider.GetRequiredService<IMovementAuthorityValidator>();
                scope.ServiceProvider.GetRequiredService<IMovementAuthorityProvider>();

                scope.ServiceProvider.GetRequiredService<SecurityManager>();

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
