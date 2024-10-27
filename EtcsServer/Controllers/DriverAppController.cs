using EtcsServer.Configuration;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionMakers;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors;
using EtcsServer.InMemoryData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;
using static EtcsServer.Controllers.TestController;
using static EtcsServer.DecisionMakers.MovementAuthorityValidator;

namespace EtcsServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class DriverAppController : ControllerBase
    {
         private readonly ILogger<TestController> _logger;

        public DriverAppController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterTrain(TrainDto train, [FromServices] RegisteredTrainsTracker registeredTrainsTracker)
        {
            _logger.LogInformation("Received register request for train {}", train.TrainId);
            bool result = registeredTrainsTracker.Register(train);
            if (result)
                return Ok(new JsonResponse() { message = $"Train with id {train.TrainId} was successfully registered" });
            else
                return BadRequest(new JsonResponse() { message = $"Train with id {train.TrainId} is already registered on the server" });
        }

        [HttpPost("updatedata")]
        public async Task<ActionResult> UpdateTrainData(UpdateTrain updateTrain, [FromServices] RegisteredTrainsTracker registeredTrainsTracker)
        {
            _logger.LogInformation("Received train data update request for train {}", updateTrain.TrainId);
            bool result = registeredTrainsTracker.Update(updateTrain);
            if (result)
                return Ok(new JsonResponse() { message = $"Successfully updated information about the train with id {updateTrain.TrainId}" });
            else
                return BadRequest(new JsonResponse() { message = $"Couldn't update information for train with id {updateTrain.TrainNumer}" });
        }

        [HttpPost("unregister")]
        public async Task<ActionResult> UnregisterTrain(UnregisterRequest unregisterRequest, [FromServices] RegisteredTrainsTracker registeredTrainsTracker)
        {
            _logger.LogInformation("Received train unregister request for train {}", unregisterRequest.TrainId);
            bool result = registeredTrainsTracker.Unregister(unregisterRequest.TrainId);
            if (result)
                return Ok(new JsonResponse() { message = $"Train with id {unregisterRequest.TrainId} was successfully unregistered" });
            else
                return BadRequest(new JsonResponse() { message = $"Train with id {unregisterRequest.TrainId} is not registered on the server" }); 
        }

        [HttpPost("updateposition")]
        public async Task<ActionResult> UpdateTrainPosition(TrainPosition trainPosition, [FromServices] LastKnownPositionsTracker positionsTracker)
        {
            _logger.LogInformation("Received train position for train {}: {}", trainPosition.TrainId, trainPosition.Kilometer);
            positionsTracker.RegisterTrainPosition(trainPosition);
            return Ok(new JsonResponse() { message = "Server received the location of train with id " + trainPosition.TrainId});
        }

        [HttpPost("marequest")]
        public async Task<ActionResult> PostMovementAuthorityRequest(
            MovementAuthorityRequest movementAuthorityRequest,
            [FromServices] MovementAuthorityValidator movementAuthorityValidator,
            [FromServices] MovementAuthorityProvider movementAuthorityProvider
            )
        {
            _logger.LogInformation("Received movement authority request for train {}", movementAuthorityRequest.TrainId);

            MovementAuthorityValidationOutcome validationOutcome = movementAuthorityValidator.IsTrainValidForMovementAuthority(movementAuthorityRequest.TrainId);
            if (validationOutcome.Result != MovementAuthorityValidationResult.OK)
                return BadRequest(new JsonResponse() { message = $"Train with id {movementAuthorityRequest.TrainId} is not valid to receive movement authority: {validationOutcome.Result}" });

            MovementAuthority movementAuthority = movementAuthorityProvider.ProvideMovementAuthority(movementAuthorityRequest.TrainId, validationOutcome.NextTrack!);
            return Ok(new JsonResponse() { message = $"Train with id {movementAuthorityRequest.TrainId} was granted a movement authority: " + JsonSerializer.Serialize(movementAuthority)});                
        }

        [HttpPost("speedupdate")]
        public async Task<ActionResult> PostTrainSpeed(TrainSpeed trainSpeed)
        {
            _logger.LogInformation("Received train speed ({}) for train {}", trainSpeed.Speed, trainSpeed.TrainId);
            return Ok(new JsonResponse() { message = "Server received the speed report for train " + trainSpeed.TrainId });
        }
    }
}
