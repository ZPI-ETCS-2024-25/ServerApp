namespace EtcsServer.DriverAppDto
{
    public class MovementAuthority
    {
        public int[] Speeds { get; set; }
        public int[] SpeedDistances { get; set; }
        public int[] Gradients { get; set; }
        public int[] GradientDistances { get; set; }
        public string[] Messages { get; set; }
        public int[] MessageDistances { get; set; }
        public int ServerPosition { get; set; }
    }
}
