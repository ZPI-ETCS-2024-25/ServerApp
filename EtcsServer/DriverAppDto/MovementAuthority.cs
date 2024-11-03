namespace EtcsServer.DriverAppDto
{
    public class MovementAuthority
    {
        public double[] Speeds { get; set; }
        public double[] SpeedDistances { get; set; }
        public double[] Gradients { get; set; }
        public double[] GradientDistances { get; set; }
        public string[] Messages { get; set; }
        public double[] MessageDistances { get; set; }
        public int[] Lines { get; set; }
        public double[] LinesDistances { get; set; }
        public double ServerPosition { get; set; }
    }
}
