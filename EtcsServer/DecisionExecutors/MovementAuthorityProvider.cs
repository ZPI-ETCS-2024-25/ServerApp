using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors;
using EtcsServer.Helpers;
using EtcsServer.InMemoryHolders;

namespace EtcsServer.DecisionExecutors
{
    public class MovementAuthorityProvider
    {
        private readonly LastKnownPositionsTracker lastKnownPositionsTracker;
        private readonly RailroadSignsHolder railroadSignsHolder;
        private readonly TrackHelper trackHelper;

        public MovementAuthorityProvider(
            LastKnownPositionsTracker lastKnownPositionsTracker,
            RailroadSignsHolder railroadSignsHolder,
            TrackHelper trackHelper
            )
        {
            this.lastKnownPositionsTracker = lastKnownPositionsTracker;
            this.railroadSignsHolder = railroadSignsHolder;
            this.trackHelper = trackHelper;
        }

        public MovementAuthority ProvideMovementAuthorityToEtcsBorder(string trainId)
        {
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            bool isMovingUp = trainPosition.Direction.Equals("up");

            List<RailroadSign> _signs = [];
            List<double> _signsDistances = [];
            List<double> _gradients = [];
            List<double> _gradientsDistances = [];

            Track currentTrack = trackHelper.GetTrackByTrainPosition(trainPosition)!;
            while (currentTrack != null && currentTrack.TrackPosition == TrackPosition.INSIDE_ZONE)
            {
                List<RailroadSign> currentSigns = railroadSignsHolder.GetValues().Values
                    .Where(s => s.TrackId == currentTrack.TrackageElementId)
                    .Where(s => s.IsFacedUp = trainPosition.Direction.Equals("up"))
                    .ToList();
                _signs.AddRange(currentSigns);
                _signsDistances.AddRange(currentSigns.Select(s => s.DistanceFromTrackStart));

                _gradients.Add(currentTrack.Gradient);
                _gradientsDistances.Add(currentTrack.Kilometer);

                currentTrack = trackHelper.GetNextTrack(currentTrack.TrackageElementId, isMovingUp)!;
            }

            return new MovementAuthority()
            {
                Speeds = _signs.Select(s => s.MaxSpeed).ToArray(),
                SpeedDistances = _signsDistances.ToArray(),
                Gradients = _gradients.ToArray(),
                GradientDistances = _gradientsDistances.ToArray(),
                Messages = ["Travel safe"],
                MessageDistances = [trainPosition.Kilometer],
                ServerPosition = trainPosition.Kilometer
            };
        }

        public MovementAuthority ProvideMovementAuthority(string trainId, RailwaySignal stopSignal)
        {
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            bool isMovingUp = trainPosition.Direction.Equals("up");

            List<RailroadSign> _signs = [];
            List<double> _signsDistances = [];
            List<double> _gradients = [];
            List<double> _gradientsDistances = [];

            Track currentTrack = trackHelper.GetTrackByTrainPosition(trainPosition)!;
            while (currentTrack.TrackageElementId != stopSignal.TrackId)
            {
                List<RailroadSign> currentSigns = railroadSignsHolder.GetValues().Values
                    .Where(s => s.TrackId == currentTrack.TrackageElementId)
                    .Where(s => s.IsFacedUp = trainPosition.Direction.Equals("up"))
                    .ToList();
                _signs.AddRange(currentSigns);
                _signsDistances.AddRange(currentSigns.Select(s => s.DistanceFromTrackStart));

                _gradients.Add(currentTrack.Gradient);
                _gradientsDistances.Add(currentTrack.Kilometer);

                currentTrack = trackHelper.GetNextTrack(currentTrack.TrackageElementId, isMovingUp)!;
            }

            return new MovementAuthority()
            {
                Speeds = _signs.Select(s => s.MaxSpeed).ToArray(),
                SpeedDistances = _signsDistances.ToArray(),
                Gradients = _gradients.ToArray(),
                GradientDistances = _gradientsDistances.ToArray(),
                Messages = ["Travel safe"],
                MessageDistances = [trainPosition.Kilometer],
                ServerPosition = trainPosition.Kilometer
            };
        }

        public MovementAuthority ProvideMovementAuthority(string trainId, Track nextTrack)
        {
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;

            List<RailroadSign> signs = railroadSignsHolder.GetValues().Values
                    .Where(s => s.Track.TrackNumber.Equals(nextTrack.TrackNumber))
                    .Where(s => s.IsFacedUp = trainPosition.Direction.Equals("up"))
                    .ToList();
            Track track = trackHelper.GetTrackByTrainPosition(trainPosition)!;

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
