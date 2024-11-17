namespace EtcsServer.Database.Entity
{
    public class RailroadSign
    {
        private double distanceFromTrackStartRounded;
        public int RailroadSignId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public double DistanceFromTrackStart
        {
            get { return distanceFromTrackStartRounded; }
            set { distanceFromTrackStartRounded = Double.Round(value, 3); }
        }
        public bool IsFacedUp { get; set; }
        public double MaxSpeed { get; set; }

        public int GetDistanceFromStartMeters() => Convert.ToInt32(DistanceFromTrackStart * 1000);
    }
}
