using EtcsServer.Configuration;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using static EtcsServer.Controllers.TestController;

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
        public async Task<ActionResult> RegisterTrain(Train train)
        {
            _logger.LogInformation("Received register request for train {}", train.TrainId);
            return Ok(new JsonResponse() { message = "Server received the register request for train " + train.TrainId });
        }

        [HttpPost("updatedata")]
        public async Task<ActionResult> UpdateTrainData(UpdateTrain updateTrain)
        {
            _logger.LogInformation("Received train data update request for train {}", updateTrain.TrainId);
            return Ok(new JsonResponse() { message = "Server received the train data update request for train " + updateTrain.TrainId });
        }

        [HttpPost("unregister")]
        public async Task<ActionResult> UnregisterTrain(UnregisterRequest unregisterRequest)
        {
            _logger.LogInformation("Received train unregister request for train {}", unregisterRequest.TrainId);
            return Ok(new JsonResponse() { message = "Server received the train unregister request for train " + unregisterRequest.TrainId });
        }

        [HttpPost("updateposition")]
        public async Task<ActionResult> UpdateTrainPosition(TrainPosition trainPosition, [FromServices] LastKnownPositionsTracker positionsTracker)
        {
            _logger.LogInformation("Received train position for train {}: {}", trainPosition.TrainId, trainPosition.Kilometer);
            positionsTracker.RegisterTrainPosition(trainPosition);
            return Ok(new JsonResponse() { message = "Server received the location of train with id " + trainPosition.TrainId});
        }

        [HttpPost("marequest")]
        public async Task<ActionResult> PostMovementAuthorityRequest(MovementAuthorityRequest movementAuthorityRequest)
        {
            _logger.LogInformation("Received movement authority request for train {}", movementAuthorityRequest.TrainId);
            return Ok(new JsonResponse() { message = "Server received the movement authority request for train " + movementAuthorityRequest.TrainId });
        }

        [HttpPost("speedupdate")]
        public async Task<ActionResult> PostTrainSpeed(TrainSpeed trainSpeed)
        {
            _logger.LogInformation("Received train speed ({}) for train {}", trainSpeed.Speed, trainSpeed.TrainId);
            return Ok(new JsonResponse() { message = "Server received the speed report for train " + trainSpeed.TrainId });
        }
    }
}
