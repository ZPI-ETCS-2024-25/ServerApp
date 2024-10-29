namespace EtcsServer.Database.Entity
{
    public class Track : TrackageElement
    {
        public string TrackNumber { get; set; }
        public int LineNumber { get; set; }
        public double Kilometer { get; set; }
        public double Length { get; set; }
        public double MaxUpSpeedMps { get; set; }
        public double MaxDownSpeedMps { get; set; }
        public double Gradient {  get; set; }
        public TrackPosition TrackPosition { get; set; }

    }

    public enum TrackPosition
    {
        INSIDE_ZONE,
        INCOMING_ZONE,
        OUTSIDE_ZONE
    }
}
