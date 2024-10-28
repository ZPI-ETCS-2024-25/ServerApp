using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors;
using EtcsServer.InMemoryHolders;

namespace EtcsServer.DecisionExecutors
{
    public class MovementAuthorityProvider
    {
        private readonly LastKnownPositionsTracker lastKnownPositionsTracker;
        private readonly RailroadSignsHolder railroadSignsHolder;
        private readonly TracksHolder tracksHolder;

        public MovementAuthorityProvider(
            LastKnownPositionsTracker lastKnownPositionsTracker,
            RailroadSignsHolder railroadSignsHolder,
            TracksHolder tracksHolder
            )
        {
            this.lastKnownPositionsTracker = lastKnownPositionsTracker;
            this.railroadSignsHolder = railroadSignsHolder;
            this.tracksHolder = tracksHolder;
        }

        public MovementAuthority ProvideMovementAuthority(string trainId, Track nextTrack)
        {
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;

            List<RailroadSign> signs = railroadSignsHolder.GetValues().Values
                    .Where(s => s.Track.TrackNumber.Equals(nextTrack.TrackNumber))
                    .Where(s => s.IsFacedUp = trainPosition.Direction.Equals("up"))
                    .ToList();
            Track track = tracksHolder.GetValues().Values.First(t => t.TrackNumber.Equals(nextTrack.TrackNumber));

            return new MovementAuthority()
            {
                Speeds = signs.Select(s => s.MaxSpeed).ToArray(),
                SpeedDistances = signs.Select(s => s.DistanceFromTrackStart).ToArray(),
                Gradients = [track.Gradient],
                GradientDistances = [track.Kilometer],
                Messages = ["You are in the middle of the track"],
                MessageDistances = [track.Kilometer + track.Length / 2],
                ServerPosition = trainPosition.Kilometer
            };
        }
    }
}
