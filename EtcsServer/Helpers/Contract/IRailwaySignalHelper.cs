using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData.Contract;

namespace EtcsServer.Helpers.Contract
{
    public interface IRailwaySignalHelper
    {
        RailwaySignal? GetFirstStopSignal(TrainPosition trainPosition, bool isMovingUp);
        RailwaySignalMessage GetMessageForSignal(int signalId);
    }
}
