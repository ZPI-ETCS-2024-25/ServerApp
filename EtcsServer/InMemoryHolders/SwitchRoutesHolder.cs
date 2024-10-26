﻿using EtcsServer.Database;
using EtcsServer.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace EtcsServer.InMemoryHolders
{
    public class SwitchRoutesHolder : Holder<SwitchRoute>
    {
        public SwitchRoutesHolder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Dictionary<int, SwitchRoute> LoadValues(EtcsDbContext context)
        {
            return context.TrackSwitches.ToDictionary(sr => sr.SwitchRouteId, sr => sr);
        }
    }
}