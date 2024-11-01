using EtcsServer.Database;
using EtcsServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace EtcsServer.InMemoryHolders
{
    public class RailroadSignsHolder : Holder<RailroadSign>
    {
        public RailroadSignsHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, RailroadSign> LoadValues(EtcsDbContext context)
        {
            return context.Signs
                .Include(s => s.Track)
                .ToDictionary(s => s.RailroadSignId, s => s);
        }
    }
}
