namespace EtcsServer.Database.Entity
{
    public class RailwaySignal
    {
        public int RailwaySignalId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public double DistanceFromTrackStart { get; set; }
        public bool IsFacedUp { get; set; }
        public int GetDistanceFromStartMeters() => Convert.ToInt32(DistanceFromTrackStart * 1000);
    }
}
