using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors;
using EtcsServer.Helpers;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;
using static EtcsServer.DriverDataCollectors.LastKnownPositionsTracker;

namespace EtcsServer.DecisionMakers
{
    public class MovementAuthorityValidator
    {
        private readonly LastKnownPositionsTracker lastKnownPositionsTracker;
        private readonly TrackHelper trackHelper;
        private readonly RailwaySignalHelper railwaySignalHelper;

        public MovementAuthorityValidator(LastKnownPositionsTracker lastKnownPositionsTracker, TrackHelper trackHelper, RailwaySignalHelper railwaySignalHelper)
        {
            this.lastKnownPositionsTracker = lastKnownPositionsTracker;
            this.trackHelper = trackHelper;
            this.railwaySignalHelper = railwaySignalHelper;
        }

        public MovementAuthorityValidationOutcome IsTrainValidForMovementAuthority(string trainId)
        {
            TrainPosition? trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId);
            if (trainPosition == null)
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.POSITION_NOT_KNOWN);

            MovementDirection movementDirection = lastKnownPositionsTracker.GetMovementDirection(trainId);
            if (movementDirection == MovementDirection.UNKNOWN)
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.MOVEMENT_DIRECTION_NOT_KNOWN);

            Track? nextTrack = trackHelper.GetNextTrack(trainPosition.Track, movementDirection == MovementDirection.UP);
            if (nextTrack == null)
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.END_OF_ROAD);

            RailwaySignalTrack? railwaySignalTrack = railwaySignalHelper.GetFirstSignalForTrack(trainPosition, nextTrack.TrackageElementId, movementDirection == MovementDirection.UP);
            if (railwaySignalTrack == null)
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.NO_RAILWAY_SIGNAL_TO_USE);

            RailwaySignalMessage message = railwaySignalHelper.GetMessageForSignal(railwaySignalTrack.RailwaySignalId);
            if (message == RailwaySignalMessage.STOP)
                return MovementAuthorityValidationOutcome.GetFailedOutcome(MovementAuthorityValidationResult.NEXT_TRACK_OCCUPIED);

            return new MovementAuthorityValidationOutcome()
            {
                Result = MovementAuthorityValidationResult.OK,
                TrainPosition = trainPosition,
                NextTrack = nextTrack
            };
        }

        public class MovementAuthorityValidationOutcome
        {
            public MovementAuthorityValidationResult Result { get; set; }
            public TrainPosition? TrainPosition { get; set; }
            public Track? NextTrack { get; set; }

            public static MovementAuthorityValidationOutcome GetFailedOutcome(MovementAuthorityValidationResult result) => new MovementAuthorityValidationOutcome() { Result = result };
        }

        public enum MovementAuthorityValidationResult
        {
            OK,
            POSITION_NOT_KNOWN,
            MOVEMENT_DIRECTION_NOT_KNOWN,
            END_OF_ROAD,
            NEXT_TRACK_OCCUPIED,
            NO_RAILWAY_SIGNAL_TO_USE
        }
    }
}
