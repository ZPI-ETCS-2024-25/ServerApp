using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace EtcsServer.InMemoryData
{
    public class MovementAuthorityTracker : IMovementAuthorityTracker
    {
        private readonly ITrainPositionTracker trainPositionTracker;
        private readonly ITrackHelper trackHelper;
        private readonly IHolder<CrossingTrack> crossingTracksHolder;
        private readonly IHolder<RailwaySignal> railwaySignalHolder;
        private Dictionary<string, MovementAuthority> trainToMovementAuthority = [];
        private Dictionary<string, List<TrackageElement>> trainToTrackageElements = [];

        public MovementAuthorityTracker(
            ITrainPositionTracker trainPositionTracker,
            ITrackHelper trackHelper,
            IHolder<CrossingTrack> crossingTracksHolder,
            IHolder<RailwaySignal> railwaySignalHolder)
        {
            this.trainPositionTracker = trainPositionTracker;
            this.trackHelper = trackHelper;
            this.crossingTracksHolder = crossingTracksHolder;
            this.railwaySignalHolder = railwaySignalHolder;
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
            List<string> impactedTrains = trainToMovementAuthority.Where(kvp => trainToTrackageElements[kvp.Key].Any(t => t.TrackageElementId == switchId))
                .Select(kvp => kvp.Key)
                .ToList();

            Dictionary<string, Track?> currentPositions = impactedTrains.ToDictionary(trainId => trainId, trainId => trackHelper.GetTrackByTrainPosition(trainPositionTracker.GetLastKnownTrainPosition(trainId)!));

            return impactedTrains
                .Where(trainId => currentPositions[trainId] != null)
                .Where(trainId => trainToTrackageElements[trainId].FindIndex(t => t.TrackageElementId == currentPositions[trainId]!.TrackageElementId) < trainToTrackageElements[trainId].FindIndex(t => t.TrackageElementId == switchId))
                .Select(trainId => (trainId, trainToMovementAuthority[trainId]))
                .ToList();
        }

        public List<(string, MovementAuthority)> GetMovementAuthoritiesImpactedByCrossing(int crossingId)
        {
            Dictionary<int, double> affectedTracksToDistances = crossingTracksHolder.GetValues().Values
                .Where(crossingTrack => crossingTrack.CrossingId == crossingId)
                .ToDictionary(crossingTrack => crossingTrack.TrackId, crossingTrack => crossingTrack.DistanceFromTrackStart);
            List<int> affectedTracks = affectedTracksToDistances.Keys.ToList();

            List<string> possiblyAffectedTrains = trainToMovementAuthority
                .Where(kvp => trainToTrackageElements[kvp.Key].Any(t => affectedTracks.Contains(t.TrackageElementId)))
                .Select(kvp => kvp.Key)
                .ToList();

            Dictionary<string, TrainPosition> currentPositionsReports = possiblyAffectedTrains.ToDictionary(trainId => trainId, trainId => trainPositionTracker.GetLastKnownTrainPosition(trainId)!);
            Dictionary<string, Track?> currentPositions = possiblyAffectedTrains.ToDictionary(trainId => trainId, trainId => trackHelper.GetTrackByTrainPosition(currentPositionsReports[trainId]));
            Dictionary<string, MovementDirection> currentDirections = possiblyAffectedTrains.ToDictionary(trainId => trainId, trainId => trainPositionTracker.GetMovementDirection(trainId)!);

            List<string> trainsImpactedOnCurrentTrack = possiblyAffectedTrains
                .Where(trainId => currentDirections[trainId] != MovementDirection.UNKNOWN && currentPositions[trainId] != null)
                .Where(trainId => affectedTracks.Contains(currentPositions[trainId]!.TrackageElementId))
                .Where(trainId => currentDirections[trainId] == MovementDirection.UP ?
                        currentPositionsReports[trainId].Kilometer < currentPositions[trainId]!.Kilometer + affectedTracksToDistances[currentPositions[trainId]!.TrackageElementId] :
                        currentPositionsReports[trainId].Kilometer > currentPositions[trainId]!.Kilometer + affectedTracksToDistances[currentPositions[trainId]!.TrackageElementId])
                .ToList();

            List<string> trainImpactedOnFutureTracks = possiblyAffectedTrains
                .Where(trainId => currentDirections[trainId] != MovementDirection.UNKNOWN && currentPositions[trainId] != null && !trainsImpactedOnCurrentTrack.Contains(trainId))
                .Where(trainId => trainToTrackageElements[trainId]
                                    .Skip(1 + trainToTrackageElements[trainId].FindIndex(e => e.TrackageElementId == currentPositions[trainId]!.TrackageElementId))
                                    .Any(e => affectedTracks.Contains(e.TrackageElementId)))
                .ToList();
            
            return trainsImpactedOnCurrentTrack.Concat(trainImpactedOnFutureTracks).Select(trainId => (trainId, trainToMovementAuthority[trainId])).ToList();
        }

        public List<(string, MovementAuthority)> GetMovementAuthoritiesImpactedByRailwaySignal(int signalId)
        {
            RailwaySignal railwaySignal = railwaySignalHolder.GetValues()[signalId];

            List<string> possiblyAffectedTrains = trainToMovementAuthority
                .Where(kvp => trainToTrackageElements[kvp.Key].Any(t => t.TrackageElementId == railwaySignal.TrackId))
                .Select(kvp => kvp.Key)
                .ToList();

            Dictionary<string, TrainPosition> currentPositionsReports = possiblyAffectedTrains.ToDictionary(trainId => trainId, trainId => trainPositionTracker.GetLastKnownTrainPosition(trainId)!);
            Dictionary<string, Track?> currentPositions = possiblyAffectedTrains.ToDictionary(trainId => trainId, trainId => trackHelper.GetTrackByTrainPosition(currentPositionsReports[trainId]));
            Dictionary<string, MovementDirection> currentDirections = possiblyAffectedTrains.ToDictionary(trainId => trainId, trainId => trainPositionTracker.GetMovementDirection(trainId)!);

            List<string> trainsImpactedOnCurrentTrack = possiblyAffectedTrains
                .Where(trainId => currentDirections[trainId] != MovementDirection.UNKNOWN && currentPositions[trainId] != null)
                .Where(trainId => currentPositions[trainId]!.TrackageElementId == railwaySignal.TrackId)
                .Where(trainId => currentDirections[trainId] == MovementDirection.UP ?
                        currentPositionsReports[trainId].Kilometer < currentPositions[trainId]!.Kilometer + railwaySignal.DistanceFromTrackStart :
                        currentPositionsReports[trainId].Kilometer > currentPositions[trainId]!.Kilometer + railwaySignal.DistanceFromTrackStart)
                .ToList();

            List<string> trainImpactedOnFutureTracks = possiblyAffectedTrains
                .Where(trainId => currentDirections[trainId] != MovementDirection.UNKNOWN && currentPositions[trainId] != null && !trainsImpactedOnCurrentTrack.Contains(trainId))
                .Where(trainId => trainToTrackageElements[trainId]
                                    .Skip(1 + trainToTrackageElements[trainId].FindIndex(e => e.TrackageElementId == currentPositions[trainId]!.TrackageElementId))
                                    .Any(e => e.TrackageElementId == railwaySignal.TrackId))
                .ToList();

            return trainsImpactedOnCurrentTrack.Concat(trainImpactedOnFutureTracks).Select(trainId => (trainId, trainToMovementAuthority[trainId])).ToList();
        }
    }
}
