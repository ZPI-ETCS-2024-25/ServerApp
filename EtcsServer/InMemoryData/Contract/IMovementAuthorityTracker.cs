using EtcsServer.DriverAppDto;

namespace EtcsServer.InMemoryData.Contract
{
    public interface IMovementAuthorityTracker
    {
        void SetActiveMovementAuthority(string trainId, MovementAuthority movementAuthority);
        MovementAuthority? GetActiveMovementAuthority(string trainId);
        List<(string, MovementAuthority)> GetActiveMovementAuthorities();
    }
}
