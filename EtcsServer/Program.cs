
using EtcsServer.Configuration;
using EtcsServer.Database;
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


            builder.Services.AddSingleton<CrossingsHolder>();
            builder.Services.AddSingleton<RailroadSignsHolder>();
            builder.Services.AddSingleton<RailwaySignalTrackHolder>();
            builder.Services.AddSingleton<SwitchRoutesHolder>();
            builder.Services.AddSingleton<TracksHolder>();
            builder.Services.AddSingleton<TrainsHolder>();

            builder.Services.Configure<ServerProperties>(builder.Configuration.GetSection("ServerProperties"));
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<CrossingsHolder>();
                scope.ServiceProvider.GetRequiredService<RailroadSignsHolder>();
                scope.ServiceProvider.GetRequiredService<RailwaySignalTrackHolder>();
                scope.ServiceProvider.GetRequiredService<SwitchRoutesHolder>();
                scope.ServiceProvider.GetRequiredService<TracksHolder>();
                scope.ServiceProvider.GetRequiredService<TrainsHolder>();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
