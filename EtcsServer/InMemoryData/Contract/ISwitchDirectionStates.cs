using EtcsServer.Database.Entity;
using EtcsServer.InMemoryHolders;

namespace EtcsServer.InMemoryData.Contract
{
    public interface ISwitchDirectionStates
    {
        SwitchDirection? GetSwitchDirectionInformation(int switchId);
    }
}
