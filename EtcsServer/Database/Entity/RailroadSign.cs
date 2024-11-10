namespace EtcsServer.Database.Entity
{
    public class RailroadSign
    {
        public int RailroadSignId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public double DistanceFromTrackStart { get; set; }
        public bool IsFacedUp { get; set; }
        public double MaxSpeed { get; set; }

        public int GetDistanceFromStartMeters() => Convert.ToInt32(DistanceFromTrackStart * 1000);
    }
}
