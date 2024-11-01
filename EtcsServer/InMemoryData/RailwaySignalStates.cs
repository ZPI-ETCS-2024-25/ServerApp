﻿using EtcsServer.Database.Entity;
using EtcsServer.InMemoryHolders;
using Microsoft.AspNetCore.Mvc;

namespace EtcsServer.InMemoryData
{
    public class RailwaySignalStates
    {
        private readonly Dictionary<int, RailwaySignalMessage> states;

        public RailwaySignalStates([FromServices] RailwaySignalsHolder signalTrackHolder)
        {
            states = [];
            signalTrackHolder.GetValues().Values.ToList()
                .ForEach(railwaySignalTrack => states.Add(railwaySignalTrack.RailwaySignalId, RailwaySignalMessage.GO));
            states[4] = RailwaySignalMessage.STOP;
            //states[5] = RailwaySignalMessage.STOP;
        }

        public void SetRailwaySignalState(int signalId, RailwaySignalMessage message)
        {
            states[signalId] = message;
        }

        public RailwaySignalMessage GetSignalMessage(int signalId)
        {
            return states[signalId];
        }
    }
    public enum RailwaySignalMessage
    {
        STOP,
        GO
    }

}
