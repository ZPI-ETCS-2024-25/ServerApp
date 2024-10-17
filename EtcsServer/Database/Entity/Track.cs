namespace EtcsServer.Database.Entity
{
    public class Track : TrackageElement
    {
        public int Kilometer { get; set; }
        public int Length { get; set; }
        public int MaxUpSpeedMps { get; set; }
        public int MaxDownSpeedMps { get; set; }
        public int Gradient {  get; set; }

    }
}
