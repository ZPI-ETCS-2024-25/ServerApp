namespace EtcsServer.Database.Entity
{
    public class RailwaySignal
    {
        public int RailwaySignalId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public double DistanceFromTrackStart { get; set; }
        public bool IsFacedUp { get; set; }
    }
}
