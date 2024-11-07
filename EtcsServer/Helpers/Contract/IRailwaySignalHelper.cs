using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData.Contract;

namespace EtcsServer.Helpers.Contract
{
    public interface IRailwaySignalHelper
    {
        RailwaySignal? GetFirstStopSignal(TrainPosition trainPosition, MovementDirection movementDirection);
        RailwaySignalMessage GetMessageForSignal(int signalId);
    }
}
