namespace EtcsServer.Database.Entity
{
    public class Train
    {
        public int TrainId { get; set; }
        public int LengthMeters { get; set; }
        public int MaxSpeedMps { get; set; }
        public int BrakeWeight { get; set; }
    }
}
