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

        public SwitchStates([FromServices] IHolder<SwitchRoute> switchRoutesHolder)
        {
            states = [];
            switchRoutesHolder.GetValues().Values.ToList()
                .GroupBy(sr => new { sr.SwitchId, sr.TrackFromId })
                .Select(group => group.First())
                .ToList()
                .ForEach(switchRoute => states.Add((switchRoute.SwitchId, switchRoute.TrackFromId), new SwitchFromTo(switchRoute.TrackFromId, switchRoute.TrackToId)));
            this.switchRoutesHolder = switchRoutesHolder;
        }

        public void SetSwitchState(int switchId, SwitchFromTo switchFromTo)
        {
            states[(switchId, switchFromTo.TrackIdFrom)] = switchFromTo;
        }

        public int GetNextTrackId(int switchId, int trackIdFrom)
        {
            return states[(switchId, trackIdFrom)].TrackIdTo;
        }

        public double? GetMaxSpeed(int switchId, int trackIdFrom)
        {
            return switchRoutesHolder.GetValues().Values
                .Where(sr => sr.SwitchId == switchId)
                .Where(sr => sr.TrackFromId == trackIdFrom)
                .FirstOrDefault()?.MaxSpeedMps;
        }
    }
}
