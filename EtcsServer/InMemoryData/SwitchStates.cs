using EtcsServer.Database.Entity;
using EtcsServer.InMemoryHolders;
using Microsoft.AspNetCore.Mvc;

namespace EtcsServer.InMemoryData
{
    public class SwitchStates
    {
        private readonly Dictionary<int, SwitchFromTo> states;

        public SwitchStates([FromServices] SwitchRoutesHolder switchRoutesHolder)
        {
            states = [];
            switchRoutesHolder.GetValues().Values.ToList()
                .GroupBy(sr => new { sr.SwitchId, sr.TrackFromId })
                .Select(group => group.First())
                .ToList()
                .ForEach(switchRoute => states.Add(switchRoute.SwitchId, new SwitchFromTo(switchRoute.TrackFromId, switchRoute.TrackToId)));
        }

        public void SetSwitchState(int switchId, SwitchFromTo switchFromTo)
        {
            states[switchId] = switchFromTo;
        }

        public int GetNextTrackId(int switchId)
        {
            return states[switchId].TrackIdTo;
        }
    }
    public class SwitchFromTo(int trackFromId, int trackToId)
    {
        public int TrackIdFrom { get; set; } = trackFromId;
        public int TrackIdTo { get; set; } = trackToId;
    }

}
