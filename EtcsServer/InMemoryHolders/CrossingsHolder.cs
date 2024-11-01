using EtcsServer.Database;
using EtcsServer.Database.Entity;

namespace EtcsServer.InMemoryHolders
{
    public class CrossingsHolder : Holder<Crossing>
    {
        public CrossingsHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, Crossing> LoadValues(EtcsDbContext context)
        {
            return context.Crossings.ToDictionary(c => c.CrossingId, c => c);
        }
    }
}
