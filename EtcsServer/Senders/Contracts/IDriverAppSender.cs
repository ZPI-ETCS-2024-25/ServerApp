using EtcsServer.DriverAppDto;

namespace EtcsServer.Senders.Contracts
{
    public interface IDriverAppSender
    {
        Task SendNewMovementAuthority(string trainId, MovementAuthority movementAuthority);
    }
}
