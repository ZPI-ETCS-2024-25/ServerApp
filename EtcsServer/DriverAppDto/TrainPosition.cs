﻿namespace EtcsServer.DriverAppDto
{
    public class TrainPosition
    {
        public string TrainId { get; set; }
        public string Kilometer { get; set; }
        public string Track { get; set; }
        public int LineNumber { get; set; }
        public string Direction { get; set; }
    }
}