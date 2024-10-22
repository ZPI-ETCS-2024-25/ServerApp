namespace EtcsServer.Database.Entity
{
    public class Track : TrackageElement
    {
        public string TrackNumber { get; set; }
        public int LineNumber { get; set; }
        public int Kilometer { get; set; }
        public int Length { get; set; }
        public int MaxUpSpeedMps { get; set; }
        public int MaxDownSpeedMps { get; set; }
        public int Gradient {  get; set; }

    }
}
