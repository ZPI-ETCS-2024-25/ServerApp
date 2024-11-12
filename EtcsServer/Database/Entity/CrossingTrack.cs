namespace EtcsServer.Database.Entity
{
    public class CrossingTrack
    {
        public int CrossingTrackId { get; set; }
        public int CrossingId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public double DistanceFromTrackStart { get; set; }
    }
}
