using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors;
using EtcsServer.Helpers;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;

namespace EtcsServer.DecisionExecutors
{
    public class MovementAuthorityProvider
    {
        private readonly LastKnownPositionsTracker lastKnownPositionsTracker;
        private readonly RailroadSignsHolder railroadSignsHolder;
        private readonly TrackHelper trackHelper;
        private readonly SwitchStates switchStates;

        public MovementAuthorityProvider(
            LastKnownPositionsTracker lastKnownPositionsTracker,
            RailroadSignsHolder railroadSignsHolder,
            TrackHelper trackHelper,
            SwitchStates switchStates
            )
        {
            this.lastKnownPositionsTracker = lastKnownPositionsTracker;
            this.railroadSignsHolder = railroadSignsHolder;
            this.trackHelper = trackHelper;
            this.switchStates = switchStates;
        }

        public MovementAuthority ProvideMovementAuthorityToEtcsBorder(string trainId)
        {
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            bool isMovingUp = trainPosition.Direction.Equals("up");
            Track? track = trackHelper.GetTrackByTrainPosition(trainPosition);
            bool wasInsideEtcsBorder = false;

            MovementAuthorityContainer authorityContainer = new()
            {
                StartingTrackId = track!.TrackageElementId,
                CurrentKilometer = trainPosition.Kilometer,
                CurrentTrack = track
            };

            while (!wasInsideEtcsBorder || (authorityContainer.CurrentTrack != null && authorityContainer.CurrentTrack.TrackPosition == TrackPosition.INSIDE_ZONE))
            {
                if (authorityContainer.CurrentTrack.TrackPosition == TrackPosition.INSIDE_ZONE)
                    wasInsideEtcsBorder = true;

                RegisterSpeedsForCurrentTrack(authorityContainer, isMovingUp);

                authorityContainer.RegisterGradient(authorityContainer.CurrentTrack.Gradient, authorityContainer.DistanceSoFar);

                UpdateTravelledDistance(authorityContainer, isMovingUp, trainPosition);

                HandleNextTrackageElement(authorityContainer, isMovingUp);
            }

            return authorityContainer.CreateMovementAuthority(trainPosition);
        }

        public MovementAuthority ProvideMovementAuthority(string trainId, RailwaySignal stopSignal)
        {
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            bool isMovingUp = trainPosition.Direction.Equals("up");
            Track? track = trackHelper.GetTrackByTrainPosition(trainPosition);

            MovementAuthorityContainer authorityContainer = new()
            {
                StartingTrackId = track!.TrackageElementId,
                CurrentKilometer = trainPosition.Kilometer,
                CurrentTrack = track
            };

            while (authorityContainer.CurrentTrack.TrackageElementId != stopSignal.TrackId)
            {
                RegisterSpeedsForCurrentTrack(authorityContainer, isMovingUp);

                authorityContainer.RegisterGradient(authorityContainer.CurrentTrack.Gradient, authorityContainer.DistanceSoFar);

                UpdateTravelledDistance(authorityContainer, isMovingUp, trainPosition);

                HandleNextTrackageElement(authorityContainer, isMovingUp);
            }

            UpdateMovementAuthorityUntilStopSignalIsReached(authorityContainer, isMovingUp, trainPosition, stopSignal);

            return authorityContainer.CreateMovementAuthority(trainPosition);
        }

        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, bool isMovingUp) => RegisterSpeedsForCurrentTrack(authorityContainer, isMovingUp, -1);
        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, bool isMovingUp, RailwaySignal stopSignal) => RegisterSpeedsForCurrentTrack(authorityContainer, isMovingUp, stopSignal.DistanceFromTrackStart);

        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, bool isMovingUp, double stopKilometerOnTrack)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            double currentKilometer = authorityContainer.CurrentKilometer;
            double distanceSoFar = authorityContainer.DistanceSoFar;
            double currentTrackMaxSpeed = isMovingUp ? currentTrack.MaxUpSpeedMps : currentTrack.MaxDownSpeedMps;
            if (stopKilometerOnTrack == -1)
                stopKilometerOnTrack = isMovingUp ? currentTrack.Length : 0;

            List<RailroadSign> signsOnCurrentTrack = railroadSignsHolder.GetValues().Values
                .Where(s => s.TrackId == currentTrack.TrackageElementId)
                .Where(s => s.IsFacedUp == isMovingUp)
                .Where(s => s.MaxSpeed < currentTrackMaxSpeed)
                .Where(s => isMovingUp ? s.DistanceFromTrackStart <= stopKilometerOnTrack : s.DistanceFromTrackStart >= stopKilometerOnTrack)
                .OrderBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                .ToList();
            List<RailroadSign> signsAhead = signsOnCurrentTrack.Where(s => isMovingUp ? s.DistanceFromTrackStart > currentKilometer : s.DistanceFromTrackStart < currentKilometer).ToList();

            double startingPointMaxSpeed = currentTrackMaxSpeed;
            RailroadSign? previousSign = signsOnCurrentTrack.LastOrDefault(s => isMovingUp ? s.DistanceFromTrackStart <= currentKilometer : s.DistanceFromTrackStart >= currentKilometer);
            if (previousSign != null)
                startingPointMaxSpeed = previousSign.MaxSpeed;

            authorityContainer.RegisterSpeed(startingPointMaxSpeed, distanceSoFar);
            signsAhead.ForEach(s => authorityContainer.RegisterSpeed(
                s.MaxSpeed,
                isMovingUp ? distanceSoFar + (s.DistanceFromTrackStart + currentTrack.Kilometer - currentKilometer) : distanceSoFar + (currentKilometer - s.DistanceFromTrackStart - currentTrack.Kilometer)
            ));
        }

        private void UpdateTravelledDistance(MovementAuthorityContainer authorityContainer, bool isMovingUp, TrainPosition originalTrainPosition)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            if (currentTrack.TrackageElementId == authorityContainer.StartingTrackId)
                authorityContainer.DistanceSoFar += isMovingUp ? (currentTrack.Kilometer + currentTrack.Length) - originalTrainPosition.Kilometer : originalTrainPosition.Kilometer - currentTrack.Kilometer;
            else authorityContainer.DistanceSoFar += currentTrack.Length;
        }

        private void UpdateTravelledDistance(MovementAuthorityContainer authorityContainer, bool isMovingUp, TrainPosition originalTrainPosition, RailwaySignal stopSignal)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            if (currentTrack.TrackageElementId == authorityContainer.StartingTrackId)
                authorityContainer.DistanceSoFar += isMovingUp ? stopSignal.DistanceFromTrackStart - originalTrainPosition.Kilometer : originalTrainPosition.Kilometer - stopSignal.DistanceFromTrackStart;
            else authorityContainer.DistanceSoFar += isMovingUp ? stopSignal.DistanceFromTrackStart : currentTrack.Length - stopSignal.DistanceFromTrackStart;
        }

        private void HandleNextTrackageElement(MovementAuthorityContainer authorityContainer, bool isMovingUp)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            TrackageElement? nextTrackageElement = trackHelper.GetNextTrackageElement(currentTrack.TrackageElementId, isMovingUp);
            if (nextTrackageElement is Switch trainSwitch)
            {
                RegisterMaxSpeedForSwitch(trainSwitch, authorityContainer);
                int nextTrackId = switchStates.GetNextTrackId(trainSwitch.TrackageElementId, currentTrack.TrackageElementId);
                authorityContainer.CurrentTrack = trackHelper.GetTrackById(nextTrackId)!;
                authorityContainer.CurrentKilometer = authorityContainer.CurrentTrack.Kilometer;
            }
            else if (nextTrackageElement is Track track)
            {
                authorityContainer.CurrentTrack = track;
                authorityContainer.CurrentKilometer = track.Kilometer;
            }
            else authorityContainer.CurrentTrack = null;
        }

        private void RegisterMaxSpeedForSwitch(Switch trainSwitch, MovementAuthorityContainer authorityContainer)
        {
            double distanceSoFar = authorityContainer.DistanceSoFar;
            double? switchMaxSpeed = switchStates.GetMaxSpeed(trainSwitch.TrackageElementId, authorityContainer.CurrentTrack!.TrackageElementId);
            double? maxSpeedBeforeSwitch = authorityContainer.Speeds.LastOrDefault();

            if (switchMaxSpeed.HasValue)
            {
                authorityContainer.RegisterSpeed(switchMaxSpeed.Value, distanceSoFar);
                if (maxSpeedBeforeSwitch.HasValue)
                    authorityContainer.RegisterSpeedNoDistanceConstraint(maxSpeedBeforeSwitch.Value, distanceSoFar);
            }
        }

        private void UpdateMovementAuthorityUntilStopSignalIsReached(MovementAuthorityContainer authorityContainer, bool isMovingUp, TrainPosition trainPosition, RailwaySignal stopSignal)
        {
            RegisterSpeedsForCurrentTrack(authorityContainer, isMovingUp, stopSignal);
            authorityContainer.RegisterGradient(authorityContainer.CurrentTrack!.Gradient, authorityContainer.DistanceSoFar);
            UpdateTravelledDistance(authorityContainer, isMovingUp, trainPosition, stopSignal);
        }

        class MovementAuthorityContainer
        {
            public List<double> Speeds { get; set; } = [];
            public List<double> SpeedsDistances { get; set; } = [];
            public List<double> Gradients { get; set; } = [];
            public List<double> GradientsDistances { get; set; } = [];

            public int StartingTrackId { get; set; }
            public double CurrentKilometer { get; set; }
            public double DistanceSoFar { get; set; }
            public Track? CurrentTrack { get; set; }

            public void RegisterSpeed(double speed, double distance)
            {
                if (Speeds.Count == 0 || speed != Speeds.Last())
                {
                    if (SpeedsDistances.Count == 0 || distance != SpeedsDistances.Last())
                    {
                        Speeds.Add(speed);
                        SpeedsDistances.Add(distance);
                    } else
                    {
                        Speeds[Speeds.Count - 1] = Math.Min(Speeds.Last(), speed);
                    }
                }
            }

            public void RegisterSpeedNoDistanceConstraint(double speed, double distance)
            {
                if (Speeds.Count == 0 || speed != Speeds.Last())
                {
                    Speeds.Add(speed);
                    SpeedsDistances.Add(distance);
                }
            }

            public void RegisterGradient(double gradient, double distance)
            {
                if (Gradients.Count == 0 || gradient != Gradients.Last())
                {
                    Gradients.Add(gradient);
                    GradientsDistances.Add(distance);
                }
            }

            public MovementAuthority CreateMovementAuthority(TrainPosition trainPosition)
            {
                RegisterSpeed(0, DistanceSoFar);
                return new MovementAuthority()
                {
                    Speeds = Speeds.ToArray(),
                    SpeedDistances = SpeedsDistances.ToArray(),
                    Gradients = Gradients.ToArray(),
                    GradientDistances = GradientsDistances.ToArray(),
                    Messages = ["Travel safe"],
                    MessageDistances = [0],
                    ServerPosition = trainPosition.Kilometer //TODO kilometer without track number and line number is ambiguous
                };
            }
        }
    }
}
