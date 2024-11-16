using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;

namespace EtcsServer.InMemoryData.Contract
{
    public interface IMovementAuthorityTracker
    {
        void SetActiveMovementAuthority(string trainId, MovementAuthority movementAuthority, List<TrackageElement> trackageElements);
        MovementAuthority? GetActiveMovementAuthority(string trainId);
        List<(string, MovementAuthority)> GetActiveMovementAuthorities();
        List<(string, MovementAuthority)> GetMovementAuthoritiesImpactedBySwitch(int switchId);
        List<(string, MovementAuthority)> GetMovementAuthoritiesImpactedByCrossing(int crossingId);
    }
}
