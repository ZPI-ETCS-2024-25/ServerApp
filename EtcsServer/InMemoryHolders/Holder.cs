using EtcsServer.Database;

namespace EtcsServer.InMemoryHolders
{
    public abstract class Holder<T>: IHolder<T>
    {
        private readonly Dictionary<int, T> values;

        public Holder(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EtcsDbContext>();
                this.values = LoadValues(dbContext);
            }
        }

        protected abstract Dictionary<int, T> LoadValues(EtcsDbContext context);

        public Dictionary<int, T> GetValues()
        {
            return values;
        }
    }
}
