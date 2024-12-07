using EtcsServer.Database.Entity;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.UnityDto;
using Microsoft.AspNetCore.Mvc;

namespace EtcsServer.Controllers
{
    [ApiController]
    [Route("/unity")]
    public class UnityAppController : ControllerBase
    {
        private readonly IMovementAuthorityTracker movementAuthorityTracker;
        private readonly IDriverAppSenderHelper driverAppSenderHelper;

        public UnityAppController(IMovementAuthorityTracker movementAuthorityTracker, IDriverAppSenderHelper driverAppSenderHelper)
        {
            this.movementAuthorityTracker = movementAuthorityTracker;
            this.driverAppSenderHelper = driverAppSenderHelper;
        }

        [HttpPost("JunctionState")]
        public async Task<ActionResult> ChangeSwitchState(JunctionStateChange junctionStateChange, [FromServices] ISwitchStates switchStates, ISwitchDirectionStates switchDirectionStates)
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

                List<string> impactedTrains = movementAuthorityTracker.GetTrainsImpactedBySwitch(junctionId);
                driverAppSenderHelper.SendUpdatedMaToEachImpactedTrain(impactedTrains);
            }
            return Ok();
        }

        [HttpPost("crossingState")]
        public async Task<ActionResult> ChangeCrossingState(CrossingStateChange crossingStateChange, [FromServices] ICrossingStates crossingStates)
        {
            (int crossingId, bool isFunctional) = (crossingStateChange.CrossingId, crossingStateChange.IsFunctional);
            bool isCurrentlyFunctional = crossingStates.GetCrossingState(crossingId);
            if (isCurrentlyFunctional != isFunctional)
            {
                crossingStates.SetCrossingState(crossingId, isFunctional);

                List<string> impactedTrains = movementAuthorityTracker.GetTrainsImpactedByCrossing(crossingId);
                driverAppSenderHelper.SendUpdatedMaToEachImpactedTrain(impactedTrains);
            }
            return Ok();
        }

        [HttpPost("semaphoreState")]
        public async Task<ActionResult> ChangeRailwaySignalState(SignalStateChange signalStateChange, [FromServices] IRailwaySignalStates railwaySignalStates)
        {
            int semaphoreId = signalStateChange.SemaphoreId;
            bool go = signalStateChange.Go;
            bool isGoMessage = go;
            RailwaySignalMessage currentMessage = railwaySignalStates.GetSignalMessage(semaphoreId);
            RailwaySignalMessage newMessage = isGoMessage ? RailwaySignalMessage.GO : RailwaySignalMessage.STOP;
            if (currentMessage != newMessage)
            {
                railwaySignalStates.SetRailwaySignalState(semaphoreId, newMessage);

                List<string> impactedTrains = movementAuthorityTracker.GetTrainsImpactedByRailwaySignal(semaphoreId);
                driverAppSenderHelper.SendUpdatedMaToEachImpactedTrain(impactedTrains);
            }
            return Ok();
        }
    }
}
