using EtcsServer.InMemoryData;

namespace EtcsServer.Database.Entity
{
    public class Track : TrackageElement
    {
        public string TrackNumber { get; set; }
        public int LineNumber { get; set; }
        public double Kilometer { get; set; }
        public double Length { get; set; }
        public double MaxUpSpeed { get; set; }
        public double MaxDownSpeed { get; set; }
        public double Gradient {  get; set; }
        public TrackPosition TrackPosition { get; set; }

        public double GetMaxSpeed(TrackEnd startingTrackEnd) => startingTrackEnd == TrackEnd.LEFT ? MaxUpSpeed : MaxDownSpeed;
        public int GetMeter() => Convert.ToInt32(Kilometer * 1000);
        public int GetLengthMeters() => Convert.ToInt32(Length * 1000);
    }
}
