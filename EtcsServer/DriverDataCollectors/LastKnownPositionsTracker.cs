using EtcsServer.Controllers;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;

namespace EtcsServer.DriverDataCollectors
{
    public partial class LastKnownPositionsTracker : ITrainPositionTracker
    {
        private readonly Dictionary<string, List<TrainPosition>> trainsPositions;
        private readonly ILogger<LastKnownPositionsTracker> logger;

        public LastKnownPositionsTracker(ILogger<LastKnownPositionsTracker> logger)
        {
            this.trainsPositions = new Dictionary<string, List<TrainPosition>>();
            this.logger = logger;
        }

        public void RegisterTrainPosition(TrainPosition trainLocation)
        {
            logger.LogInformation("Saving train location in memory");
            if (!trainsPositions.ContainsKey(trainLocation.TrainId))
                trainsPositions.Add(trainLocation.TrainId, [trainLocation]);
            else
                trainsPositions[trainLocation.TrainId].Add(trainLocation);
        }

        public TrainPosition? GetLastKnownTrainPosition(string trainId) {
            if (!trainsPositions.TryGetValue(trainId, out List<TrainPosition>? positionsList))
                return null;
            return positionsList.Last();
        }

        public MovementDirection GetMovementDirection(string trainId)
        {
            TrainPosition? lastKnownTrainPosition = GetLastKnownTrainPosition(trainId);
            if (lastKnownTrainPosition == null)
                return MovementDirection.UNKNOWN;
            return lastKnownTrainPosition.Direction switch
            {
                "up" => MovementDirection.UP,
                "down" => MovementDirection.DOWN,
                _ => MovementDirection.UNKNOWN
            };
        }
    }
}
