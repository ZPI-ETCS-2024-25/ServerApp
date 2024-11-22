using EtcsServer.Database.Entity;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;

namespace EtcsServer.InMemoryData
{
    public class SwitchDirectionStates : ISwitchDirectionStates
    {
        private readonly IHolder<SwitchDirection> switchDirectionHolder;

        public SwitchDirectionStates(IHolder<SwitchDirection> switchDirectionHolder)
        {
            this.switchDirectionHolder = switchDirectionHolder;
        }

        public SwitchDirection? GetSwitchDirectionInformation(int switchId)
        {
            if (switchDirectionHolder.GetValues().TryGetValue(switchId, out SwitchDirection? switchDirection))
                return switchDirection;
            return null;
        }
    }
}
