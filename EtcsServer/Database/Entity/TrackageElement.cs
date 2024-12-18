﻿using EtcsServer.InMemoryData;

namespace EtcsServer.Database.Entity
{
    public abstract class TrackageElement
    {
        public int TrackageElementId { get; set; }
        public int? LeftSideElementId { get; set; }
        public TrackageElement? LeftSideElement { get; set; }
        public int? RightSideElementId { get; set; }
        public TrackageElement? RightSideElement { get; set ; }

        public TrackageElement? GetNext(TrackEnd trackEnd) => trackEnd == TrackEnd.RIGHT ? RightSideElement : LeftSideElement;
    }
}
