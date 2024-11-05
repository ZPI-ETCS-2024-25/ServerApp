using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static EtcsServer.Controllers.TestController;
using static EtcsServer.DecisionMakers.Contract.MovementAuthorityValidationOutcome;

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
        public async Task<ActionResult> RegisterTrain(TrainDto train, [FromServices] IRegisteredTrainsTracker registeredTrainsTracker)
        {
            _logger.LogInformation("Received register request for train {}", train.TrainId);
            bool result = registeredTrainsTracker.Register(train);
            if (result)
                return Ok(new RegisterTrainResponse());
            else
                return BadRequest(new RegisterTrainResponse() { RegisterSuccess = false });
        }

        [HttpPost("updatedata")]
        public async Task<ActionResult> UpdateTrainData(UpdateTrain updateTrain, [FromServices] IRegisteredTrainsTracker registeredTrainsTracker)
        {
            _logger.LogInformation("Received train data update request for train {}", updateTrain.TrainId);
            bool result = registeredTrainsTracker.Update(updateTrain);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("unregister")]
        public async Task<ActionResult> UnregisterTrain(UnregisterRequest unregisterRequest, [FromServices] IRegisteredTrainsTracker registeredTrainsTracker)
        {
            _logger.LogInformation("Received train unregister request for train {}", unregisterRequest.TrainId);
            bool result = registeredTrainsTracker.Unregister(unregisterRequest.TrainId);
            return result ? Ok(new UnregisterTrainResponse()) : BadRequest(new UnregisterTrainResponse());
        }

        [HttpPost("updateposition")]
        public async Task<ActionResult> UpdateTrainPosition(TrainPosition trainPosition, [FromServices] ITrainPositionTracker positionsTracker)
        {
            _logger.LogInformation("Received train position for train {}: {}", trainPosition.TrainId, trainPosition.Kilometer);
            positionsTracker.RegisterTrainPosition(trainPosition);
            return Ok(new JsonResponse() { message = "Server received the location of train with id " + trainPosition.TrainId});
        }

        [HttpPost("marequest")]
        public async Task<ActionResult> PostMovementAuthorityRequest(
            MovementAuthorityRequest movementAuthorityRequest,
            [FromServices] IMovementAuthorityValidator movementAuthorityValidator,
            [FromServices] IMovementAuthorityProvider movementAuthorityProvider
            )
        {
            _logger.LogInformation("Received movement authority request for train {}", movementAuthorityRequest.TrainId);

            MovementAuthorityValidationOutcome validationOutcome = movementAuthorityValidator.IsTrainValidForMovementAuthority(movementAuthorityRequest.TrainId);
            if (validationOutcome.Result != MovementAuthorityValidationResult.OK)
                return BadRequest(new JsonResponse() { message = $"Train with id {movementAuthorityRequest.TrainId} is not valid to receive movement authority: {validationOutcome.Result}" });

            MovementAuthority movementAuthority = validationOutcome.NextStopSignal == null ?
                movementAuthorityProvider.ProvideMovementAuthorityToEtcsBorder(movementAuthorityRequest.TrainId) :
                movementAuthorityProvider.ProvideMovementAuthority(movementAuthorityRequest.TrainId, validationOutcome.NextStopSignal!);
            
            return Ok(movementAuthority);                
        }

        [HttpPost("speedupdate")]
        public async Task<ActionResult> PostTrainSpeed(TrainSpeed trainSpeed)
        {
            _logger.LogInformation("Received train speed ({}) for train {}", trainSpeed.Speed, trainSpeed.TrainId);
            return Ok();
        }
    }
}
