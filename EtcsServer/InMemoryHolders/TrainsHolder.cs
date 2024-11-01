﻿using EtcsServer.Database;
using EtcsServer.Database.Entity;

namespace EtcsServer.InMemoryHolders
{
    public class TrainsHolder : Holder<Train>
    {
        public TrainsHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, Train> LoadValues(EtcsDbContext context)
        {
            return context.Trains.ToDictionary(t => t.TrainId, t => t);
        }
    }
}
