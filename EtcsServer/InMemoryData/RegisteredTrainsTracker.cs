using EtcsServer.Controllers;
using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryHolders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace EtcsServer.InMemoryData
{
    public class RegisteredTrainsTracker
    {
        private readonly Dictionary<string, TrainDto> trains;
        private readonly ILogger<RegisteredTrainsTracker> logger;

        public RegisteredTrainsTracker(ILogger<RegisteredTrainsTracker> logger)
        {
            trains = [];
            this.logger = logger;
        }

        public bool Register(TrainDto train)
        {
            if (trains.ContainsKey(train.TrainId))
                return false;
            logger.LogInformation("Registering train with id {}", train.TrainId);
            trains.Add(train.TrainId, train);
            return true;
        }

        public bool Update(UpdateTrain updateTrain)
        {
            if (!trains.ContainsKey(updateTrain.TrainNumer))
                return false;

            TrainDto updatedTrain = new TrainDto()
            {
                TrainId = updateTrain.TrainId,
                LengthMeters = updateTrain.LengthMeters,
                MaxSpeed = updateTrain.MaxSpeed,
                BrakeWeight = updateTrain.BrakeWeight
            };

            logger.LogInformation("Updating train with id {}", updateTrain.TrainNumer);
            logger.LogInformation("Previous data: {}", JsonSerializer.Serialize(trains[updateTrain.TrainNumer]));
            logger.LogInformation("New data: {}", JsonSerializer.Serialize(updatedTrain));

            if (updateTrain.TrainNumer != updateTrain.TrainId)
                trains.Remove(updateTrain.TrainNumer);
            trains[updateTrain.TrainNumer] = updatedTrain;

            logger.LogInformation("Update successful");
            return true;
        }

        public bool Unregister(string trainId)
        {
            if (!trains.ContainsKey(trainId))
                return false;
            logger.LogInformation("Unregistering train with id {}", trainId);
            trains.Remove(trainId);
            return true;
        }
    }

}
