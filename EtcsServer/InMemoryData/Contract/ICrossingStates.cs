using EtcsServer.Database.Entity;

namespace EtcsServer.InMemoryData.Contract
{
    public interface ICrossingStates
    {
        void SetCrossingState(int crossingId, bool isFunctional);
        bool GetCrossingState(int crossingId);
        List<CrossingTrack> GetDamagedCrossingTracks(int trackId);
    }
}
