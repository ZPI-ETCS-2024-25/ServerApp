namespace EtcsServer.Database.Entity
{
    public class Crossing
    {
        public int CrossingId { get; set; }
        public int TrackId { get; set; }
        public Track Track { get; set; }
    }
}
