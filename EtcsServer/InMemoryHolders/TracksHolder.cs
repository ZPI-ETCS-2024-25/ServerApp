using EtcsServer.Database;
using EtcsServer.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace EtcsServer.InMemoryHolders
{
    public class TracksHolder : Holder<Track>
    {
        public TracksHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, Track> LoadValues(EtcsDbContext context)
        {
            return context.Tracks.ToDictionary(t => t.TrackageElementId, t => t);
        }
    }
}
