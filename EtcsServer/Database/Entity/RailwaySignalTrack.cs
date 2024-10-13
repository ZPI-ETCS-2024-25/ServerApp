namespace EtcsServer.Database.Entity
{
    public class RailwaySignalTrack
    {
        public int RailwaySignalTrackId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public int RailwaySignalId { get; set; }
        public RailwaySignal RailwaySignal { get; set; }
        public int DistanceFromTrackStart { get; set; }
        public bool IsFacedUp { get; set; }
    }
}
