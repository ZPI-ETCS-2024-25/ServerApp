using EtcsServer.Database;
using EtcsServer.Database.Entity;

namespace EtcsServer.InMemoryHolders
{
    public class SwitchDirectionHolder : Holder<SwitchDirection>
    {
        public SwitchDirectionHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, SwitchDirection> LoadValues(EtcsDbContext context)
        {
            return context.SwitchDirections.ToDictionary(sd => sd.SwitchDirectionId, sd => sd);
        }
    }
}
