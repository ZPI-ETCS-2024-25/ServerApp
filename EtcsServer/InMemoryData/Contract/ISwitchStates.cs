namespace EtcsServer.InMemoryData.Contract
{
    public interface ISwitchStates
    {
        void SetSwitchState(int switchId, SwitchFromTo switchFromTo);
        void SetSwitchState(int switchId, bool isGoingStraight);
        int GetNextTrackId(int switchId, int trackIdFrom);
    }
}
