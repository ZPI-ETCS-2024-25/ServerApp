using EtcsServer.Database;
using EtcsServer.Database.Entity;

namespace EtcsServer.MapLoading
{
    public class MapLoader : IMapLoader
    {
        public void LoadMapIntoDatabase(EtcsDbContext context, ITrackageMap map)
        {
            map.TrackageElementsLookup.Values.ToList().ForEach(element => context.Set<TrackageElement>().Add(element));
            map.RailwaySignalsLookup.Values.ToList().ForEach(signal => context.TrackSignals.Add(signal));
            map.SwitchRoutesLookup.Values.ToList().ForEach(switchRoute => context.TrackSwitches.Add(switchRoute));
            map.CrossingsLookup.Values.ToList().ForEach(crossing => context.Crossings.Add(crossing));
            map.CrossingTracksLookup.Values.ToList().ForEach(crossingTrack => context.CrossingTracks.Add(crossingTrack));

            context.SaveChanges();
        }
    }
}
