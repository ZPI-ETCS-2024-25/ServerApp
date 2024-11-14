using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData.Contract;

namespace EtcsServer.InMemoryData
{
    public class MovementAuthorityTracker : IMovementAuthorityTracker
    {
        private Dictionary<string, MovementAuthority> trainToMovementAuthority = [];

        public void SetActiveMovementAuthority(string trainId, MovementAuthority movementAuthority)
        {
            trainToMovementAuthority[trainId] = movementAuthority;
        }

        public MovementAuthority? GetActiveMovementAuthority(string trainId)
        {
            return trainToMovementAuthority[trainId];
        }

        public List<(string, MovementAuthority)> GetActiveMovementAuthorities()
        {
            return trainToMovementAuthority.Select(kvp => (kvp.Key, kvp.Value)).ToList();
        }
    }
}
