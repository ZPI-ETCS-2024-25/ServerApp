using EtcsServer.DriverAppDto;

namespace EtcsServer.DriverDataCollectors.Contract
{
    public interface ITrainPositionTracker
    {
        void RegisterTrainPosition(TrainPosition trainLocation);
        TrainPosition? GetLastKnownTrainPosition(string trainId);
        MovementDirection GetMovementDirection(string trainId);
    }
}
