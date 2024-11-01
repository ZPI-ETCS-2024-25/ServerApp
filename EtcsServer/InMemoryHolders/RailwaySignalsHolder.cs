using EtcsServer.Database;
using EtcsServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace EtcsServer.InMemoryHolders
{
    public class RailwaySignalsHolder : Holder<RailwaySignal>
    {
        public RailwaySignalsHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, RailwaySignal> LoadValues(EtcsDbContext context)
        {
            return context.TrackSignals
                .Include(ts => ts.Track)
                .ToDictionary(t => t.RailwaySignalId, t => t);
        }
    }
}
