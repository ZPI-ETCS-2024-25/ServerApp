using EtcsServer.Database.Entity;
using EtcsServer.InMemoryHolders;
using Microsoft.AspNetCore.Mvc;

namespace EtcsServer.InMemoryData
{
    public class SwitchStates
    {
        private readonly Dictionary<(int, int), SwitchFromTo> states;
        private readonly SwitchRoutesHolder switchRoutesHolder;

        public SwitchStates([FromServices] SwitchRoutesHolder switchRoutesHolder)
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
    public class SwitchFromTo(int trackFromId, int trackToId)
    {
        public int TrackIdFrom { get; set; } = trackFromId;
        public int TrackIdTo { get; set; } = trackToId;
    }

}
