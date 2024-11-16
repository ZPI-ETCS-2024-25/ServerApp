using EtcsServer.Database.Entity;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using Microsoft.AspNetCore.Mvc;

namespace EtcsServer.InMemoryData
{
    public class CrossingStates : ICrossingStates
    {
        private readonly Dictionary<int, bool> states;
        private readonly IHolder<Crossing> crossingsHolder;

        public CrossingStates([FromServices] IHolder<Crossing> crossingsHolder)
        {
            states = [];
            crossingsHolder.GetValues().Values
                .ToList()
                .ForEach(crossing => states.Add(crossing.CrossingId, true));
            this.crossingsHolder = crossingsHolder;
        }

        public bool GetCrossingState(int crossingId)
        {
            return states[crossingId];
        }

        public void SetCrossingState(int crossingId, bool isFunctional)
        {
            states[crossingId] = isFunctional;
        }
    }
}
