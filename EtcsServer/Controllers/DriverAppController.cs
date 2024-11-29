using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.Security;
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
         private readonly ISecurityManager securityManager;

        public DriverAppController(ILogger<TestController> logger, ISecurityManager securityManager)
        {
            _logger = logger;
            this.securityManager = securityManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterTrain(EncryptedMessage encryptedMessage, [FromServices] IRegisteredTrainsTracker registeredTrainsTracker)
        {
            TrainDto train = securityManager.Decrypt<TrainDto>(encryptedMessage.Content);
            _logger.LogInformation("Received register request for train {}", train.TrainId);
            bool result = registeredTrainsTracker.Register(train);
            if (result)
                return Ok(GetEncryptedResponse(new RegisterTrainResponse()));
            else
                return BadRequest(GetEncryptedResponse(new RegisterTrainResponse() { RegisterSuccess = false }));
        }

        [HttpPost("updatedata")]
        public async Task<ActionResult> UpdateTrainData(EncryptedMessage encryptedMessage, [FromServices] IRegisteredTrainsTracker registeredTrainsTracker)
        {
            UpdateTrain updateTrain = securityManager.Decrypt<UpdateTrain>(encryptedMessage.Content);
            _logger.LogInformation("Received train data update request for train {}", updateTrain.TrainId);
            bool result = registeredTrainsTracker.Update(updateTrain);
            return result ? Ok() : BadRequest();
        }

        [HttpPost("unregister")]
        public async Task<ActionResult> UnregisterTrain(EncryptedMessage encryptedMessage, [FromServices] IRegisteredTrainsTracker registeredTrainsTracker)
        {
            UnregisterRequest unregisterRequest = securityManager.Decrypt<UnregisterRequest>(encryptedMessage.Content);
            _logger.LogInformation("Received train unregister request for train {}", unregisterRequest.TrainId);
            bool result = registeredTrainsTracker.Unregister(unregisterRequest.TrainId);
            return result ? Ok(GetEncryptedResponse(new UnregisterTrainResponse())) : BadRequest(GetEncryptedResponse(new UnregisterTrainResponse()));
        }

        [HttpPost("updateposition")]
        public async Task<ActionResult> UpdateTrainPosition(EncryptedMessage encryptedMessage, [FromServices] ITrainPositionTracker positionsTracker)
        {
            TrainPosition trainPosition = securityManager.Decrypt<TrainPosition>(encryptedMessage.Content);
            _logger.LogInformation("Received train position for train {}: {}", trainPosition.TrainId, trainPosition.Kilometer);
            positionsTracker.RegisterTrainPosition(trainPosition);
            return Ok(GetEncryptedResponse(new JsonResponse() { message = "Server received the location of train with id " + trainPosition.TrainId}));
        }

        [HttpPost("marequest")]
        public async Task<ActionResult> PostMovementAuthorityRequest(
            EncryptedMessage encryptedMessage,
            [FromServices] IMovementAuthorityValidator movementAuthorityValidator,
            [FromServices] IMovementAuthorityProvider movementAuthorityProvider,
            [FromServices] IMovementAuthorityTracker movementAuthorityTracker
            )
        {
            MovementAuthorityRequest movementAuthorityRequest = securityManager.Decrypt<MovementAuthorityRequest>(encryptedMessage.Content);
            _logger.LogInformation("Received movement authority request for train {}", movementAuthorityRequest.TrainId);

            MovementAuthorityValidationOutcome validationOutcome = movementAuthorityValidator.IsTrainValidForMovementAuthority(movementAuthorityRequest.TrainId);
            if (validationOutcome.Result != MovementAuthorityValidationResult.OK)
                return BadRequest(GetEncryptedResponse(new JsonResponse() { message = $"Train with id {movementAuthorityRequest.TrainId} is not valid to receive movement authority: {validationOutcome.Result}" }));

            MovementAuthority movementAuthority = validationOutcome.NextStopSignal == null ?
                movementAuthorityProvider.ProvideMovementAuthorityToEtcsBorder(movementAuthorityRequest.TrainId) :
                movementAuthorityProvider.ProvideMovementAuthority(movementAuthorityRequest.TrainId, validationOutcome.NextStopSignal!);

            MovementAuthorityWithTimestamp response = new MovementAuthorityWithTimestamp(movementAuthority);

            return Ok(GetEncryptedResponse(response));                
        }

        [HttpPost("speedupdate")]
        public async Task<ActionResult> PostTrainSpeed(EncryptedMessage encryptedMessage)
        {
            TrainSpeed trainSpeed = securityManager.Decrypt<TrainSpeed>(encryptedMessage.Content);
            _logger.LogInformation("Received train speed ({}) for train {}", trainSpeed.Speed, trainSpeed.TrainId);
            return Ok();
        }

        [HttpPost("alive")]
        public ActionResult GetHeartbeat() => Ok(GetEncryptedResponse(IsAliveResponse.GetAliveResponse()));

        private string GetEncryptedResponse(object response) => JsonSerializer.Serialize(new { Content = securityManager.Encrypt(response) });
    }
}
