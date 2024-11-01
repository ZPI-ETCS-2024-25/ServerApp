using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;

namespace EtcsServer.DecisionMakers.Contract
{
    public class MovementAuthorityValidationOutcome
    {
        public MovementAuthorityValidationResult Result { get; set; }
        public TrainPosition? TrainPosition { get; set; }
        public RailwaySignal? NextStopSignal { get; set; }

        public static MovementAuthorityValidationOutcome GetFailedOutcome(MovementAuthorityValidationResult result) => new MovementAuthorityValidationOutcome() { Result = result };

        public enum MovementAuthorityValidationResult
        {
            OK,
            POSITION_NOT_KNOWN,
            MOVEMENT_DIRECTION_NOT_KNOWN,
            END_OF_ROAD,
            NEXT_TRACK_OCCUPIED,
            NO_RAILWAY_SIGNAL_TO_USE,
            TRAIN_OUTSIDE_OF_ETCS_BORDER
        }
    }
}
