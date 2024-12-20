﻿namespace EtcsServer.Database.Entity
{
    public class SwitchingTrack : TrackageElement
    {
        public double Length { get; set; }
        public double MaxSpeed { get; set; }
        public double Gradient { get; set; }
        public TrackPosition TrackPosition { get; set; }
        public int GetLengthMeters() => Convert.ToInt32(Length * 1000);
    }
}
