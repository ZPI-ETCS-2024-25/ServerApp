using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.Security;
using EtcsServer.Senders.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static EtcsServer.Controllers.TestController;
using static EtcsServer.DecisionMakers.Contract.MovementAuthorityValidationOutcome;

namespace EtcsServer.Controllers
{
    [ApiController]
    [Route("/unity")]
    public class UnityAppController : ControllerBase
    {
         private readonly ILogger<TestController> _logger;
        private readonly IMovementAuthorityTracker movementAuthorityTracker;
        private readonly ISwitchStates switchStates;

        public UnityAppController(ILogger<TestController> logger, IMovementAuthorityTracker movementAuthorityTracker, ISwitchStates switchStates)
        {
            _logger = logger;
            this.movementAuthorityTracker = movementAuthorityTracker;
            this.switchStates = switchStates;
        }

        [HttpPost("switchState")]
        public async Task<ActionResult> ChangeSwitchState(
            int switchId,
            int trackFromId,
            int trackToId,
            [FromServices] IMovementAuthorityValidator movementAuthorityValidator,
            [FromServices] IMovementAuthorityProvider movementAuthorityProvider,
            [FromServices] IDriverAppSender driverAppSender
            )
        {
            SwitchFromTo newState = new(trackFromId, trackToId);
            int currentNextTrack = switchStates.GetNextTrackId(switchId, newState.TrackIdFrom);
            if (currentNextTrack != newState.TrackIdTo)
            {
                switchStates.SetSwitchState(switchId, newState);

                List<(string, MovementAuthority)> impactedMovementAuthorities = movementAuthorityTracker.GetMovementAuthoritiesImpactedBySwitch(switchId);

                foreach ((string trainId, MovementAuthority movementAuthority) in impactedMovementAuthorities) {
                    MovementAuthorityValidationOutcome validationOutcome = movementAuthorityValidator.IsTrainValidForMovementAuthority(trainId);
                    if (validationOutcome.Result == MovementAuthorityValidationResult.OK)
                    {
                        MovementAuthority newMovementAuthority = validationOutcome.NextStopSignal == null ?
                            movementAuthorityProvider.ProvideMovementAuthorityToEtcsBorder(trainId) :
                            movementAuthorityProvider.ProvideMovementAuthority(trainId, validationOutcome.NextStopSignal!);

                        driverAppSender.SendNewMovementAuthority(trainId, newMovementAuthority);
                    }
                }
            }
            return Ok();
        }
    }
}
