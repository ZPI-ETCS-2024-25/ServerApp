using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData.Contract;
using System.Runtime.ConstrainedExecution;

namespace EtcsServer.InMemoryData
{
    public class MovementAuthorityTracker : IMovementAuthorityTracker
    {
        private readonly ITrainPositionTracker trainPositionTracker;
        private readonly ITrackHelper trackHelper;
        private Dictionary<string, MovementAuthority> trainToMovementAuthority = [];
        private Dictionary<string, List<TrackageElement>> trainToTrackageElements = [];

        public MovementAuthorityTracker(ITrainPositionTracker trainPositionTracker, ITrackHelper trackHelper)
        {
            this.trainPositionTracker = trainPositionTracker;
            this.trackHelper = trackHelper;
        }

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

        public List<(string, MovementAuthority)> GetMovementAuthoritiesImpactedBySwitch(int switchId)
        {
            List<(string, MovementAuthority)> movementAuthorities = trainToMovementAuthority.Where(kvp => trainToTrackageElements[kvp.Key].Any(t => t.TrackageElementId == switchId))
                .Select(kvp => (kvp.Key, kvp.Value))
                .ToList();

            Dictionary<string, Track?> currentPositions = movementAuthorities.Select(kvp => (kvp.Item1, trackHelper.GetTrackByTrainPosition(trainPositionTracker.GetLastKnownTrainPosition(kvp.Item1)!))).ToDictionary(kvp => kvp.Item1, kvp => kvp.Item2);

            return movementAuthorities
                .Where(kvp => currentPositions[kvp.Item1] != null)
                .Where(kvp => trainToTrackageElements[kvp.Item1].FindIndex(t => t.TrackageElementId == currentPositions[kvp.Item1]!.TrackageElementId) < trainToTrackageElements[kvp.Item1].FindIndex(t => t.TrackageElementId == switchId))
                .ToList();
        }
    }
}
