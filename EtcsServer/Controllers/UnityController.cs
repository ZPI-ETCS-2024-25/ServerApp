using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.Security;
using EtcsServer.Senders.Contracts;
using EtcsServer.UnityDto;
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
        private readonly ICrossingStates crossingStates;
        private readonly IRailwaySignalStates railwaySignalStates;
        private readonly ISwitchDirectionStates switchDirectionStates;

        public UnityAppController(ILogger<TestController> logger, IMovementAuthorityTracker movementAuthorityTracker, ISwitchStates switchStates, ICrossingStates crossingStates, IRailwaySignalStates railwaySignalStates, ISwitchDirectionStates switchDirectionStates)
        {
            _logger = logger;
            this.movementAuthorityTracker = movementAuthorityTracker;
            this.switchStates = switchStates;
            this.crossingStates = crossingStates;
            this.railwaySignalStates = railwaySignalStates;
            this.switchDirectionStates = switchDirectionStates;
        }

        [HttpPost("JunctionState")]
        public async Task<ActionResult> ChangeSwitchState(
            JunctionStateChange junctionStateChange,
            [FromServices] IMovementAuthorityValidator movementAuthorityValidator,
            [FromServices] IMovementAuthorityProvider movementAuthorityProvider,
            [FromServices] IDriverAppSender driverAppSender
            )
        {
            int junctionId = junctionStateChange.JunctionId;
            bool straight = junctionStateChange.Straight;
            SwitchDirection? switchDirection = switchDirectionStates.GetSwitchDirectionInformation(junctionId);
            if (switchDirection == null) return Ok();

            int newTrackToId = straight ? switchDirection.TrackToIdGoingStraight : switchDirection.TrackToIdTurning;
            int currentNextTrack = switchStates.GetNextTrackId(junctionId, switchDirection.TrackFromId);
            if (currentNextTrack != newTrackToId)
            {
                switchStates.SetSwitchState(junctionId, new SwitchFromTo(switchDirection.TrackFromId, newTrackToId));

                List<(string, MovementAuthority)> impactedMovementAuthorities = movementAuthorityTracker.GetMovementAuthoritiesImpactedBySwitch(junctionId);

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

        [HttpPost("crossingState")]
        public async Task<ActionResult> ChangeCrossingState(
            int crossingId,
            bool isFunctional,
            [FromServices] IMovementAuthorityValidator movementAuthorityValidator,
            [FromServices] IMovementAuthorityProvider movementAuthorityProvider,
            [FromServices] IDriverAppSender driverAppSender
            )
        {
            bool isCurrentlyFunctional = crossingStates.GetCrossingState(crossingId);
            if (isCurrentlyFunctional != isFunctional)
            {
                crossingStates.SetCrossingState(crossingId, isFunctional);

                List<(string, MovementAuthority)> impactedMovementAuthorities = movementAuthorityTracker.GetMovementAuthoritiesImpactedByCrossing(crossingId);

                foreach ((string trainId, MovementAuthority _) in impactedMovementAuthorities)
                {
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

        [HttpPost("semaphoreState")]
        public async Task<ActionResult> ChangeRailwaySignalState(
            SignalStateChange signalStateChange,
            [FromServices] IMovementAuthorityValidator movementAuthorityValidator,
            [FromServices] IMovementAuthorityProvider movementAuthorityProvider,
            [FromServices] IDriverAppSender driverAppSender
            )
        {
            int semaphoreId = signalStateChange.SemaphoreId;
            bool go = signalStateChange.Go;
            bool isGoMessage = go;
            RailwaySignalMessage currentMessage = railwaySignalStates.GetSignalMessage(semaphoreId);
            RailwaySignalMessage newMessage = isGoMessage ? RailwaySignalMessage.GO : RailwaySignalMessage.STOP;
            if (currentMessage != newMessage)
            {
                railwaySignalStates.SetRailwaySignalState(semaphoreId, newMessage);

                List<(string, MovementAuthority)> impactedMovementAuthorities = movementAuthorityTracker.GetMovementAuthoritiesImpactedByRailwaySignal(semaphoreId);

                foreach ((string trainId, MovementAuthority _) in impactedMovementAuthorities)
                {
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
