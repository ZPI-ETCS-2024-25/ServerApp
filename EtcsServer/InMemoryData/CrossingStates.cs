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
        private readonly IHolder<CrossingTrack> crossingTracks;

        public CrossingStates([FromServices] IHolder<Crossing> crossingsHolder, [FromServices] IHolder<CrossingTrack> crossingTracks)
        {
            states = [];
            crossingsHolder.GetValues().Values
                .ToList()
                .ForEach(crossing => states.Add(crossing.CrossingId, true));
            this.crossingsHolder = crossingsHolder;
            this.crossingTracks = crossingTracks;
        }

        public bool GetCrossingState(int crossingId)
        {
            return states[crossingId];
        }

        public List<CrossingTrack> GetDamagedCrossingTracks(int trackId)
        {
            return crossingTracks.GetValues()
                .Where(kvp => !states[kvp.Value.CrossingId])
                .Where(kvp => kvp.Value.TrackId == trackId)
                .Select(kvp => kvp.Value)
                .ToList();
        }

        public List<CrossingTrack> GetCrossingTracks(int trackId)
        {
            return crossingTracks.GetValues()
                .Where(kvp => kvp.Value.TrackId == trackId)
                .Select(kvp => kvp.Value)
                .ToList();
        }

        public void SetCrossingState(int crossingId, bool isFunctional)
        {
            states[crossingId] = isFunctional;
        }
    }
}
