namespace EtcsServer.InMemoryData.Contract
{
    public interface ICrossingStates
    {
        void SetCrossingState(int crossingId, bool isFunctional);
        bool GetCrossingState(int crossingId);
    }
}
