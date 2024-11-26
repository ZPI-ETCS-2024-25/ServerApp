using EtcsServer.Database.Entity;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using Microsoft.AspNetCore.Mvc;

namespace EtcsServer.InMemoryData
{
    public class SwitchStates : ISwitchStates
    {
        private readonly Dictionary<(int, int), SwitchFromTo> states;
        private readonly IHolder<SwitchRoute> switchRoutesHolder;
        private readonly IHolder<SwitchDirection> switchDirections;

        public SwitchStates(IHolder<SwitchRoute> switchRoutesHolder, IHolder<SwitchDirection> switchDirections)
        {
            states = [];
            switchRoutesHolder.GetValues().Values.ToList()
                .GroupBy(sr => new { sr.SwitchId, sr.TrackFromId })
                .Select(group => (switchDirections.GetValues().TryGetValue(group.Key.SwitchId, out SwitchDirection? switchDirection) && switchDirection.TrackFromId == group.Key.TrackFromId) ?
                    new { group.Key.SwitchId, group.Key.TrackFromId, TrackToId = switchDirections.GetValues()[group.Key.SwitchId].TrackToIdGoingStraight } :
                    new { group.Key.SwitchId, group.Key.TrackFromId, TrackToId = group.First().TrackToId }
                )
                .ToList()
                .ForEach(switchRoute => states.Add((switchRoute.SwitchId, switchRoute.TrackFromId), new SwitchFromTo(switchRoute.TrackFromId, switchRoute.TrackToId)));
            this.switchRoutesHolder = switchRoutesHolder;
            this.switchDirections = switchDirections;
        }

        public void SetSwitchState(int switchId, SwitchFromTo switchFromTo)
        {
            states[(switchId, switchFromTo.TrackIdFrom)] = switchFromTo;
        }

        public int GetNextTrackId(int switchId, int trackIdFrom)
        {
            return states[(switchId, trackIdFrom)].TrackIdTo;
        }

        public void SetSwitchState(int switchId, bool isGoingStraight)
        {
            throw new NotImplementedException();
        }
    }
}
