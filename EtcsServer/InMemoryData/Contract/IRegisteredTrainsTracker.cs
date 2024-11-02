using EtcsServer.DriverAppDto;

namespace EtcsServer.InMemoryData.Contract
{
    public interface IRegisteredTrainsTracker
    {
        bool Register(TrainDto train);
        bool Update(UpdateTrain updateTrain);
        bool Unregister(string trainId);
        List<TrainDto> GetRegisteredTrains();
        TrainDto? GetRegisteredTrain(string trainId);
    }
}
