using EtcsServer.Database;
using EtcsServer.Database.Entity;

namespace EtcsServer.InMemoryHolders
{
    public class CrossingTracksHolder : Holder<CrossingTrack>
    {
        public CrossingTracksHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, CrossingTrack> LoadValues(EtcsDbContext context)
        {
            return context.CrossingTracks.ToDictionary(c => c.CrossingTrackId, c => c);
        }
    }
}
