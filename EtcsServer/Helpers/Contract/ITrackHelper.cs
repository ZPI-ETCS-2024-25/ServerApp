using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData;

namespace EtcsServer.Helpers.Contract
{
    public interface ITrackHelper
    {
        TrackageElement GetTrackageElement(int id);
        TrackageElement? GetNextTrackageElement(int trackId, TrackEnd trackEnd);
        Track? GetNextTrack(int trackId, MovementDirection movementDirection);
        Track? GetTrackById(int trackId);
        Track? GetTrackByTrainPosition(TrainPosition trainPosition);
    }
}
