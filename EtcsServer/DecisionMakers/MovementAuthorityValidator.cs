using EtcsServer.Database.Entity;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData.Contract;
using static EtcsServer.DecisionMakers.Contract.MovementAuthorityValidationOutcome;

namespace EtcsServer.DecisionMakers
{
    public partial class MovementAuthorityValidator : IMovementAuthorityValidator
    {
        private readonly IRegisteredTrainsTracker registeredTrainsTracker;
        private readonly ITrainPositionTracker lastKnownPositionsTracker;
        private readonly ITrackHelper trackHelper;
        private readonly IRailwaySignalHelper railwaySignalHelper;

        public MovementAuthorityValidator(
            IRegisteredTrainsTracker registeredTrainsTracker,
            ITrainPositionTracker lastKnownPositionsTracker,
            ITrackHelper trackHelper,
            IRailwaySignalHelper railwaySignalHelper
            )
        {
            this.registeredTrainsTracker = registeredTrainsTracker;
            this.lastKnownPositionsTracker = lastKnownPositionsTracker;
            this.trackHelper = trackHelper;
            this.railwaySignalHelper = railwaySignalHelper;
        }

        public MovementAuthorityValidationOutcome IsTrainValidForMovementAuthority(string trainId)
        {
            if (registeredTrainsTracker.GetRegisteredTrain(trainId) == null)
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.TRAIN_NOT_REGISTERED);

            TrainPosition? trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId);
            if (trainPosition == null)
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.POSITION_NOT_KNOWN);

            MovementDirection movementDirection = lastKnownPositionsTracker.GetMovementDirection(trainId);
            if (movementDirection == MovementDirection.UNKNOWN)
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.MOVEMENT_DIRECTION_NOT_KNOWN);

            Track currentTrack = trackHelper.GetTrackByTrainPosition(trainPosition)!;
            Track? nextTrack = trackHelper.GetNextTrack(currentTrack.TrackageElementId, movementDirection);
            if ((currentTrack.TrackPosition == TrackPosition.INCOMING_ZONE && nextTrack!.TrackPosition == TrackPosition.OUTSIDE_ZONE) ||
                (currentTrack.TrackPosition == TrackPosition.OUTSIDE_ZONE && nextTrack == null))
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.TRAIN_OUTSIDE_OF_ETCS_BORDER);

            RailwaySignal? firstStopSignal = railwaySignalHelper.GetFirstStopSignal(trainPosition, movementDirection);
            if (firstStopSignal != null && currentTrack.TrackPosition == TrackPosition.INSIDE_ZONE && trackHelper.GetTrackById(firstStopSignal.TrackId)!.TrackPosition != TrackPosition.INSIDE_ZONE)
                firstStopSignal = null;

            return new MovementAuthorityValidationOutcome()
            {
                Result = MovementAuthorityValidationResult.OK,
                TrainPosition = trainPosition,
                NextStopSignal = firstStopSignal
            };
        }

        
    }
}
