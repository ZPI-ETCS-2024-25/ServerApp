﻿using EtcsServer.Database.Entity;
using EtcsServer.InMemoryHolders;
using Microsoft.AspNetCore.Mvc;

namespace EtcsServer.InMemoryData
{
    public class RailwaySignalStates
    {
        private readonly Dictionary<int, RailwaySignalMessage> states;

        public RailwaySignalStates([FromServices] RailwaySignalTrackHolder signalTrackHolder)
        {
            states = [];
            signalTrackHolder.GetValues().Values.ToList()
                .ForEach(railwaySignalTrack => states.Add(railwaySignalTrack.RailwaySignalId, RailwaySignalMessage.STOP));
        }

        public void SetRailwaySignalState(int signalId, RailwaySignalMessage message)
        {
            states[signalId] = message;
        }
    }
    public enum RailwaySignalMessage
    {
        STOP,
        GO
    }

}
