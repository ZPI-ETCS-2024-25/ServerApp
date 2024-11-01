namespace EtcsServer.InMemoryData.Contract
{
    public interface IRailwaySignalStates
    {
        void SetRailwaySignalState(int signalId, RailwaySignalMessage message);

        RailwaySignalMessage GetSignalMessage(int signalId);
    }
}
