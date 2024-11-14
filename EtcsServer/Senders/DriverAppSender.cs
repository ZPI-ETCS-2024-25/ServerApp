using EtcsServer.DriverAppDto;
using EtcsServer.Senders.Contracts;

namespace EtcsServer.Senders
{
    public class DriverAppSender : IDriverAppSender
    {
        public Task SendNewMovementAuthority(string trainId, MovementAuthority movementAuthority)
        {
            return Task.CompletedTask;
        }
    }
}
