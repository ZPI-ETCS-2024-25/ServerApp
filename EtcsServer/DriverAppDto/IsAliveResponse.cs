namespace EtcsServer.DriverAppDto
{
    public class IsAliveResponse
    {
        public bool IsAlive { get; set; }
        public static IsAliveResponse GetAliveResponse() => new IsAliveResponse() { IsAlive = true };
    }
}
