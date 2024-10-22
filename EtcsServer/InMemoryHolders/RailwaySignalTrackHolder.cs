using EtcsServer.Database;
using EtcsServer.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace EtcsServer.InMemoryHolders
{
    public class RailwaySignalTrackHolder : Holder<RailwaySignalTrack>
    {
        public RailwaySignalTrackHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, RailwaySignalTrack> LoadValues(EtcsDbContext context)
        {
            return context.TrackSignals
                .Include(ts => ts.RailwaySignal)
                .Include(ts => ts.Track)
                .ToDictionary(t => t.RailwaySignalTrackId, t => t);
        }
    }
}
