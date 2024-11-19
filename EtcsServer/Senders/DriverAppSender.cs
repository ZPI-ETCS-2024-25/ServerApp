using EtcsServer.Configuration;
using EtcsServer.DriverAppDto;
using EtcsServer.Security;
using EtcsServer.Senders.Contracts;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EtcsServer.Senders
{
    public class DriverAppSender : IDriverAppSender
    {
        private readonly ILogger<DriverAppSender> logger;
        private readonly ISecurityManager securityManager;
        private readonly string driverAppUrl;

        public DriverAppSender(ILogger<DriverAppSender> logger, IOptions<ServerProperties> serverProperties, ISecurityManager securityManager)
        {
            this.logger = logger;
            this.securityManager = securityManager;
            this.driverAppUrl = serverProperties.Value.DriverAppUrl;
        }
        public async Task SendNewMovementAuthority(string trainId, MovementAuthority movementAuthority)
        {
            logger.LogInformation("Sending new movement authority to driver app: {}", JsonSerializer.Serialize(movementAuthority));

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(driverAppUrl);
            var message = new EncryptedPostMessage(securityManager.Encrypt(new MovementAuthorityWithTimestamp(movementAuthority)));
            var serializedMessage = JsonSerializer.Serialize(message);
            await client.PostAsync("/", new StringContent(serializedMessage));
        }
    }
}
