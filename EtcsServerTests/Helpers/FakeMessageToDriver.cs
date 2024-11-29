using EtcsServer.DriverAppDto;

namespace EtcsServerTests.Helpers
{
    public class FakeMessageToDriver
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public MovementAuthority? MovementAuthority { get; set; }
    }
}
