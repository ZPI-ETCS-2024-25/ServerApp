using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData.Contract;

namespace EtcsServer.InMemoryData
{
    public class MovementAuthorityTracker : IMovementAuthorityTracker
    {
        private Dictionary<string, MovementAuthority> trainToMovementAuthority = [];
        private Dictionary<string, List<TrackageElement>> trainToTrackageElements = [];

        public void SetActiveMovementAuthority(string trainId, MovementAuthority movementAuthority, List<TrackageElement> trackageElements)
        {
            trainToMovementAuthority[trainId] = movementAuthority;
            trainToTrackageElements[trainId] = trackageElements;
        }

        public MovementAuthority? GetActiveMovementAuthority(string trainId)
        {
            return trainToMovementAuthority[trainId];
        }

        public List<(string, MovementAuthority)> GetActiveMovementAuthorities()
        {
            return trainToMovementAuthority.Select(kvp => (kvp.Key, kvp.Value)).ToList();
        }

        public List<(string, MovementAuthority)> GetMovementAuthoritiesForGivenSwitch()
        {
            return trainToMovementAuthority.Select(kvp => (kvp.Key, kvp.Value)).ToList();
        }
    }
}
