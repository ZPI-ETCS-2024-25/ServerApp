
using EtcsServer.Configuration;
using EtcsServer.Database;
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


            builder.Services.Configure<ServerProperties>(builder.Configuration.GetSection("ServerProperties"));
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
