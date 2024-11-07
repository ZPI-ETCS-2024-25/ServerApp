using EtcsServer.Database;
using EtcsServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace EtcsServer.InMemoryHolders
{
    public class TrackageElementHolder : Holder<TrackageElement>
    {
        public TrackageElementHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, TrackageElement> LoadValues(EtcsDbContext context)
        {
            return context.Set<TrackageElement>()
                .Include(t => t.LeftSideElement)
                .Include(t => t.RightSideElement)
                .ToDictionary(t => t.TrackageElementId, t => t);
        }
    }
}
