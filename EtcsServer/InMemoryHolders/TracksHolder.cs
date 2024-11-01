using EtcsServer.Database;
using EtcsServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace EtcsServer.InMemoryHolders
{
    public class TracksHolder : Holder<Track>
    {
        public TracksHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, Track> LoadValues(EtcsDbContext context)
        {
            return context.Tracks
                .Include(t => t.LeftSideElement)
                .Include(t => t.RightSideElement)
                .ToDictionary(t => t.TrackageElementId, t => t);
        }
    }
}
