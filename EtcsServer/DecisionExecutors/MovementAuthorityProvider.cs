using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.ExtensionMethods;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using System.Runtime.ConstrainedExecution;
using System.Security.AccessControl;

namespace EtcsServer.DecisionExecutors
{
    public class MovementAuthorityProvider : IMovementAuthorityProvider
    {
        private readonly ITrainPositionTracker lastKnownPositionsTracker;
        private readonly IRegisteredTrainsTracker registeredTrainsTracker;
        private readonly IHolder<RailroadSign> railroadSignsHolder;
        private readonly ITrackHelper trackHelper;
        private readonly ISwitchStates switchStates;

        public MovementAuthorityProvider(
            ITrainPositionTracker lastKnownPositionsTracker,
            IRegisteredTrainsTracker registeredTrainsTracker,
            IHolder<RailroadSign> railroadSignsHolder,
            ITrackHelper trackHelper,
            ISwitchStates switchStates
            )
        {
            this.lastKnownPositionsTracker = lastKnownPositionsTracker;
            this.registeredTrainsTracker = registeredTrainsTracker;
            this.railroadSignsHolder = railroadSignsHolder;
            this.trackHelper = trackHelper;
            this.switchStates = switchStates;
        }

        public MovementAuthority ProvideMovementAuthorityToEtcsBorder(string trainId)
        {
            TrainDto train = registeredTrainsTracker.GetRegisteredTrain(trainId)!;
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            TrackEnd destinationTrackEnd = trainPosition.Direction.Equals("up") ? TrackEnd.RIGHT : TrackEnd.LEFT;
            Track? track = trackHelper.GetTrackByTrainPosition(trainPosition);
            bool wasInsideEtcsBorder = false;

            MovementAuthorityContainer authorityContainer = new(train)
            {
                StartingTrackId = track!.TrackageElementId,
                CurrentKilometer = trainPosition.Kilometer,
                CurrentTrack = track
            };

            while (!wasInsideEtcsBorder || (authorityContainer.CurrentTrack != null && authorityContainer.CurrentTrack.TrackPosition == TrackPosition.INSIDE_ZONE))
            {
                if (authorityContainer.CurrentTrack.TrackPosition == TrackPosition.INSIDE_ZONE)
                    wasInsideEtcsBorder = true;

                RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd);

                authorityContainer.RegisterGradient(authorityContainer.CurrentTrack.Gradient, authorityContainer.DistanceSoFar);
                authorityContainer.RegisterLine();

                UpdateTravelledDistance(authorityContainer, destinationTrackEnd, trainPosition);

                HandleNextTrackageElement(authorityContainer, destinationTrackEnd);
            }

            return authorityContainer.CreateMovementAuthority(trainPosition);
        }

        public MovementAuthority ProvideMovementAuthority(string trainId, RailwaySignal stopSignal)
        {
            TrainDto train = registeredTrainsTracker.GetRegisteredTrain(trainId)!;
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            TrackEnd destinationTrackEnd = trainPosition.Direction.Equals("up") ? TrackEnd.RIGHT : TrackEnd.LEFT;
            Track? track = trackHelper.GetTrackByTrainPosition(trainPosition);

            MovementAuthorityContainer authorityContainer = new(train)
            {
                StartingTrackId = track!.TrackageElementId,
                CurrentKilometer = trainPosition.Kilometer,
                CurrentTrack = track
            };

            while (authorityContainer.CurrentTrack.TrackageElementId != stopSignal.TrackId)
            {
                RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd);

                authorityContainer.RegisterGradient(authorityContainer.CurrentTrack.Gradient, authorityContainer.DistanceSoFar);
                authorityContainer.RegisterLine();

                UpdateTravelledDistance(authorityContainer, destinationTrackEnd, trainPosition);

                HandleNextTrackageElement(authorityContainer, destinationTrackEnd);
            }

            UpdateMovementAuthorityUntilStopSignalIsReached(authorityContainer, destinationTrackEnd, trainPosition, stopSignal);

            return authorityContainer.CreateMovementAuthority(trainPosition);
        }

        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd) => RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd, -1);
        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, RailwaySignal stopSignal) => RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd, stopSignal.DistanceFromTrackStart);

        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, double stopKilometerOnTrack)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            double currentKilometer = authorityContainer.CurrentKilometer;
            double distanceSoFar = authorityContainer.DistanceSoFar;
            bool isMovingUp = destinationTrackEnd == TrackEnd.RIGHT;
            double currentTrackMaxSpeed = isMovingUp ? currentTrack.MaxUpSpeed : currentTrack.MaxDownSpeed;
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

        private void UpdateTravelledDistance(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, TrainPosition originalTrainPosition)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            if (currentTrack.TrackageElementId == authorityContainer.StartingTrackId)
                authorityContainer.DistanceSoFar += (destinationTrackEnd == TrackEnd.RIGHT) ? (currentTrack.Kilometer + currentTrack.Length) - originalTrainPosition.Kilometer : originalTrainPosition.Kilometer - currentTrack.Kilometer;
            else authorityContainer.DistanceSoFar += currentTrack.Length;
        }

        private void UpdateTravelledDistance(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, TrainPosition originalTrainPosition, RailwaySignal stopSignal)
        {
            bool isMovingUp = destinationTrackEnd == TrackEnd.RIGHT;
            Track currentTrack = authorityContainer.CurrentTrack!;
            if (currentTrack.TrackageElementId == authorityContainer.StartingTrackId)
                authorityContainer.DistanceSoFar += isMovingUp ? stopSignal.DistanceFromTrackStart - originalTrainPosition.Kilometer : originalTrainPosition.Kilometer - stopSignal.DistanceFromTrackStart;
            else authorityContainer.DistanceSoFar += isMovingUp ? stopSignal.DistanceFromTrackStart : currentTrack.Length - stopSignal.DistanceFromTrackStart;
        }

        private void HandleNextTrackageElement(MovementAuthorityContainer authorityContainer, TrackEnd currentTrackEnd)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            TrackageElement? nextTrackageElement = trackHelper.GetNextTrackageElement(currentTrack.TrackageElementId, currentTrackEnd);
            if (nextTrackageElement is Switch trainSwitch)
            {
                HandleSwitch(trainSwitch, currentTrack.TrackageElementId, authorityContainer);
            }
            else if (nextTrackageElement is Track track)
            {
                authorityContainer.CurrentTrack = track;
                authorityContainer.CurrentKilometer = track.Kilometer;
            }
            else authorityContainer.CurrentTrack = null;
        }

        private void HandleSwitch(Switch trainSwitch, int trackFromId, MovementAuthorityContainer authorityContainer)
        {
            int nextTrackId = switchStates.GetNextTrackId(trainSwitch.TrackageElementId, trackFromId);
            TrackageElement trackageElementAfterSwitch = trackHelper.GetTrackageElement(nextTrackId);

            while (trackageElementAfterSwitch != null && trackageElementAfterSwitch is not Track)
            {
                if (trackageElementAfterSwitch is SwitchingTrack switchingTrack)
                {
                    TrackEnd currentSwitchingTrackEnd = switchingTrack.RightSideElementId == trainSwitch.TrackageElementId ? TrackEnd.RIGHT : TrackEnd.LEFT;
                    RegisterSwitchingTrack(switchingTrack, currentSwitchingTrackEnd, authorityContainer);
                    trainSwitch = (Switch)switchingTrack.GetNext(currentSwitchingTrackEnd.GetOppositeEnd())!;
                    nextTrackId = switchStates.GetNextTrackId(trainSwitch.TrackageElementId, switchingTrack.TrackageElementId);
                    trackageElementAfterSwitch = trackHelper.GetTrackageElement(nextTrackId);
                }
                else throw new Exception("Trackage element after switch is neither a track or a switching track");
            }

            if (trackageElementAfterSwitch is Track track)
            {
                authorityContainer.CurrentTrack = track;
                authorityContainer.CurrentKilometer = track.Kilometer;
            } else
            {
                authorityContainer.CurrentTrack = null;
            }
        }


        private void RegisterSwitchingTrack(SwitchingTrack switchingTrack, TrackEnd startingTrackEnd, MovementAuthorityContainer authorityContainer)
        {
            double distanceSoFar = authorityContainer.DistanceSoFar;
            double gradient = startingTrackEnd == TrackEnd.LEFT ? switchingTrack.Gradient : -1 * switchingTrack.Gradient;

            authorityContainer.RegisterSpeed(switchingTrack.MaxSpeed, distanceSoFar);
            authorityContainer.RegisterGradient(gradient, distanceSoFar);
            authorityContainer.DistanceSoFar += switchingTrack.Length;
        }

        private void UpdateMovementAuthorityUntilStopSignalIsReached(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, TrainPosition trainPosition, RailwaySignal stopSignal)
        {
            RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd, stopSignal);
            authorityContainer.RegisterGradient(authorityContainer.CurrentTrack!.Gradient, authorityContainer.DistanceSoFar);
            authorityContainer.RegisterLine();
            UpdateTravelledDistance(authorityContainer, destinationTrackEnd, trainPosition, stopSignal);
        }

        class MovementAuthorityContainer(TrainDto train)
        {
            private readonly TrainDto _train = train;

            public List<double> Speeds { get; set; } = [];
            public List<double> SpeedsDistances { get; set; } = [];
            public List<double> Gradients { get; set; } = [];
            public List<double> GradientsDistances { get; set; } = [];
            public List<int> Lines { get; set; } = [];
            public List<double> LinesDistances { get; set; } = [];

            public int StartingTrackId { get; set; }
            public double CurrentKilometer { get; set; }
            public double DistanceSoFar { get; set; }
            public Track? CurrentTrack { get; set; }

            public void RegisterSpeed(double speed, double kilometer)
            {
                int distance = (int)(kilometer * 1000);
                if (Speeds.Count > 0 && Speeds.Last() < speed)
                    distance += Int32.Parse(_train.LengthMeters) / 1000;

                if (Speeds.Count == 0 || speed != Speeds.Last())
                {
                    if (SpeedsDistances.Count == 0 || distance > SpeedsDistances.Last())
                    {
                        Speeds.Add(speed);
                        SpeedsDistances.Add(distance);
                    } else if (distance < SpeedsDistances.Last())
                    {
                        Speeds[Speeds.Count - 1] = speed;
                        SpeedsDistances[SpeedsDistances.Count - 1] = distance;
                    } else
                    {
                        Speeds[Speeds.Count - 1] = Math.Min(Speeds.Last(), speed);
                    }
                }
            }

            public void RegisterGradient(double gradient, double kilometer)
            {
                int distance = (int)(kilometer * 1000);
                if (Gradients.Count == 0 || gradient != Gradients.Last())
                {
                    Gradients.Add(gradient);
                    GradientsDistances.Add(distance);
                }
            }

            public void RegisterLine()
            {
                if (CurrentTrack != null && (Lines.Count == 0 || CurrentTrack.LineNumber != Lines.Last()))
                {
                    Lines.Add(CurrentTrack.LineNumber);
                    LinesDistances.Add(DistanceSoFar * 1000);
                }
            }

            public MovementAuthority CreateMovementAuthority(TrainPosition trainPosition)
            {
                RegisterSpeed(0, DistanceSoFar);
                if (GradientsDistances.Last() == DistanceSoFar * 1000)
                {
                    Gradients.RemoveAt(Gradients.Count-1);
                }
                else GradientsDistances.Add(DistanceSoFar * 1000);
                
                LinesDistances.Add(DistanceSoFar * 1000);
                
                return new MovementAuthority()
                {
                    Speeds = Speeds.ToArray(),
                    SpeedDistances = SpeedsDistances.ToArray(),
                    Gradients = Gradients.ToArray(),
                    GradientDistances = GradientsDistances.ToArray(),
                    Messages = [],
                    MessageDistances = [],
                    Lines = Lines.ToArray(),
                    LinesDistances = LinesDistances.ToArray(),
                    ServerPosition = trainPosition.Kilometer
                };
            }
        }
    }
}
