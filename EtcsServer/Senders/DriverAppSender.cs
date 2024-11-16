using EtcsServer.DriverAppDto;
using EtcsServer.Senders.Contracts;
using System.Text.Json;

namespace EtcsServer.Senders
{
    public class DriverAppSender : IDriverAppSender
    {
        private readonly ILogger<DriverAppSender> logger;

        public DriverAppSender(ILogger<DriverAppSender> logger)
        {
            this.logger = logger;
        }
        public Task SendNewMovementAuthority(string trainId, MovementAuthority movementAuthority)
        {
            logger.LogInformation("Sending new movement authority to driver app: {}", JsonSerializer.Serialize(movementAuthority));
            return Task.CompletedTask;
        }
    }
}
