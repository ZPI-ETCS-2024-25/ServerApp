namespace EtcsServer.Configuration
{
    public class EtcsProperties
    {
        public int MaxSpeedDamagedCrossing  { get; set; }
        public double DamagedCrossingImpactLength { get; set; }
        public int DistanceForMessageBeforeCrossing { get; set; }
        public string CrossingMessage { get; set; }
    }
}
