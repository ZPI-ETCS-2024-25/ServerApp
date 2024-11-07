namespace EtcsServer.InMemoryData.Contract
{
    public interface ISwitchStates
    {
        void SetSwitchState(int switchId, SwitchFromTo switchFromTo);
        int GetNextTrackId(int switchId, int trackIdFrom);
    }
}
