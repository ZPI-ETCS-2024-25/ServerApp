namespace EtcsServer.Database.Entity
{
    public class Train
    {
        public int TrainId { get; set; }
        public double LengthMeters { get; set; }
        public double MaxSpeed { get; set; }
        public double BrakeWeight { get; set; }
        public List<Message> Messages { get; set; }
    }
}
