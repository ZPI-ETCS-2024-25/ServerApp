using EtcsServer.Controllers;
using EtcsServer.DriverAppDto;

namespace EtcsServer.DriverDataCollectors
{
    public class LastKnownPositionsTracker
    {
        private readonly Dictionary<int, List<TrainPosition>> trainsPositions;
        private readonly ILogger<LastKnownPositionsTracker> logger;

        public LastKnownPositionsTracker(ILogger<LastKnownPositionsTracker> logger)
        {
            this.trainsPositions = new Dictionary<int, List<TrainPosition>>();
            this.logger = logger;
        }

        public void RegisterTrainPosition(TrainPosition trainLocation)
        {
            logger.LogInformation("Saving train location in memory");
            if (!trainsPositions.ContainsKey(Int32.Parse(trainLocation.TrainId)))
                trainsPositions.Add(Int32.Parse(trainLocation.TrainId), [trainLocation]);
            else
                trainsPositions[Int32.Parse(trainLocation.TrainId)].Add(trainLocation);
        }
    }
}
