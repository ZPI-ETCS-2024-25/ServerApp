using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;

namespace EtcsServer.Helpers.Contract
{
    public interface ITrackHelper
    {
        TrackageElement? GetNextTrackageElement(int trackId, bool isDirectionUp);
        Track? GetNextTrack(int trackId, bool isDirectionUp);
        Track? GetTrackById(int trackId);
        Track? GetTrackByTrainPosition(TrainPosition trainPosition);
    }
}
