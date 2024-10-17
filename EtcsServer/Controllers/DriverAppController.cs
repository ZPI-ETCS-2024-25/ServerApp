using EtcsServer.Configuration;
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

        [HttpPost]
        public async Task<ActionResult> PostTrainPosition(TrainPosition trainPosition, [FromServices] LastKnownPositionsTracker positionsTracker)
        {
            _logger.LogInformation("Received train position for train {}: {}", trainPosition.TrainId, trainPosition.Kilometer);
            positionsTracker.RegisterTrainPosition(trainPosition);
            return Ok(new JsonResponse() { message = "Server received the location of train with id " + trainPosition.TrainId});
        }
    }
}
