using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.ExtensionMethods;
using EtcsServer.Helpers.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Security.AccessControl;

namespace EtcsServer.DecisionExecutors
{
    public class MovementAuthorityProvider : IMovementAuthorityProvider
    {
        private IMovementAuthorityTracker movementAuthorityTracker;
        private readonly ITrainPositionTracker lastKnownPositionsTracker;
        private readonly IRegisteredTrainsTracker registeredTrainsTracker;
        private readonly IHolder<RailroadSign> railroadSignsHolder;
        private readonly ITrackHelper trackHelper;
        private readonly ISwitchStates switchStates;
        private readonly ICrossingStates crossingStates;

        public MovementAuthorityProvider(
            IMovementAuthorityTracker movementAuthorityTracker,
            ITrainPositionTracker lastKnownPositionsTracker,
            IRegisteredTrainsTracker registeredTrainsTracker,
            IHolder<RailroadSign> railroadSignsHolder,
            ITrackHelper trackHelper,
            ISwitchStates switchStates,
            ICrossingStates crossingStates
            )
        {
            this.movementAuthorityTracker = movementAuthorityTracker;
            this.lastKnownPositionsTracker = lastKnownPositionsTracker;
            this.registeredTrainsTracker = registeredTrainsTracker;
            this.railroadSignsHolder = railroadSignsHolder;
            this.trackHelper = trackHelper;
            this.switchStates = switchStates;
            this.crossingStates = crossingStates;
        }

        public MovementAuthority ProvideMovementAuthorityToEtcsBorder(string trainId)
        {
            TrainDto train = registeredTrainsTracker.GetRegisteredTrain(trainId)!;
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            TrackEnd destinationTrackEnd = lastKnownPositionsTracker.GetMovementDirection(trainId) == MovementDirection.UP ? TrackEnd.RIGHT : TrackEnd.LEFT;
            Track? track = trackHelper.GetTrackByTrainPosition(trainPosition);
            bool wasInsideEtcsBorder = false;

            MovementAuthorityContainer authorityContainer = new(train, trainPosition)
            {
                StartingTrackId = track!.TrackageElementId,
                CurrentMeter = trainPosition.GetMeter(),
                CurrentTrack = track,
                TrackageElements = [track!]
            };

            while (!wasInsideEtcsBorder || (authorityContainer.CurrentTrack != null && authorityContainer.CurrentTrack.TrackPosition == TrackPosition.INSIDE_ZONE))
            {
                if (authorityContainer.CurrentTrack.TrackPosition == TrackPosition.INSIDE_ZONE)
                    wasInsideEtcsBorder = true;

                RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd);

                RegisterGradient(authorityContainer, destinationTrackEnd);
                authorityContainer.RegisterLine();

                UpdateTravelledDistance(authorityContainer, destinationTrackEnd, trainPosition);

                HandleNextTrackageElement(authorityContainer, destinationTrackEnd);
            }

            double finishingSpeed = authorityContainer.CurrentTrack == null ? 0 : authorityContainer.CurrentTrack.GetMaxSpeed(destinationTrackEnd);
            return CreateMovementAuthority(authorityContainer, finishingSpeed);
        }

        public MovementAuthority ProvideMovementAuthority(string trainId, RailwaySignal stopSignal)
        {
            TrainDto train = registeredTrainsTracker.GetRegisteredTrain(trainId)!;
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            TrackEnd destinationTrackEnd = lastKnownPositionsTracker.GetMovementDirection(trainId) == MovementDirection.UP ? TrackEnd.RIGHT : TrackEnd.LEFT;
            Track? track = trackHelper.GetTrackByTrainPosition(trainPosition);

            MovementAuthorityContainer authorityContainer = new(train, trainPosition)
            {
                StartingTrackId = track!.TrackageElementId,
                CurrentMeter = trainPosition.GetMeter(),
                CurrentTrack = track,
                TrackageElements = [track!]
            };

            while (authorityContainer.CurrentTrack.TrackageElementId != stopSignal.TrackId)
            {
                RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd);

                RegisterGradient(authorityContainer, destinationTrackEnd);
                authorityContainer.RegisterLine();

                UpdateTravelledDistance(authorityContainer, destinationTrackEnd, trainPosition);

                HandleNextTrackageElement(authorityContainer, destinationTrackEnd);
            }

            UpdateMovementAuthorityUntilStopSignalIsReached(authorityContainer, destinationTrackEnd, trainPosition, stopSignal);
            return CreateMovementAuthority(authorityContainer, 0);
        }

        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd) => RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd, -1);
        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, RailwaySignal stopSignal) => RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd, stopSignal.DistanceFromTrackStart);

        private void RegisterSpeedsForCurrentTrack(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, double stopKilometerOnTrack)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            int currentMeter = authorityContainer.CurrentMeter;
            int metersSoFar = authorityContainer.MetersSoFar;
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
            List<RailroadSign> signsAhead = signsOnCurrentTrack.Where(s => isMovingUp ? s.GetDistanceFromStartMeters() > currentMeter : s.GetDistanceFromStartMeters() < currentMeter).ToList();

            double startingPointMaxSpeed = currentTrackMaxSpeed;
            RailroadSign? previousSign = signsOnCurrentTrack.LastOrDefault(s => isMovingUp ? s.GetDistanceFromStartMeters() <= currentMeter : s.GetDistanceFromStartMeters() >= currentMeter);
            if (previousSign != null)
                startingPointMaxSpeed = previousSign.MaxSpeed;

            List<CrossingTrack> damagedCrossingsOnCurrentTrack = crossingStates.GetDamagedCrossingTracks(currentTrack.TrackageElementId);
            int maxSpeedOnDamagedCrossing = 20; //TODO move out to config file
            double kilometersBeforeAndAfterCrossing = 0.01; //TODO move out to config file
            damagedCrossingsOnCurrentTrack.ForEach(crossing =>
            {
                int indexForInsert = isMovingUp ?
                    signsAhead.FindIndex(sign => sign.DistanceFromTrackStart > crossing.DistanceFromTrackStart - kilometersBeforeAndAfterCrossing) :
                    signsAhead.FindIndex(sign => sign.DistanceFromTrackStart < crossing.DistanceFromTrackStart + kilometersBeforeAndAfterCrossing);
                if (indexForInsert == -1)
                    indexForInsert = signsAhead.Count;

                signsAhead.Insert(
                indexForInsert,
                new RailroadSign() { IsFacedUp = isMovingUp, MaxSpeed = maxSpeedOnDamagedCrossing, DistanceFromTrackStart = crossing.DistanceFromTrackStart - (isMovingUp ? kilometersBeforeAndAfterCrossing : -1 * kilometersBeforeAndAfterCrossing) }
                );

                int indexForSecondInsert = isMovingUp ?
                    signsAhead.FindIndex(sign => sign.DistanceFromTrackStart > crossing.DistanceFromTrackStart + kilometersBeforeAndAfterCrossing) :
                    signsAhead.FindIndex(sign => sign.DistanceFromTrackStart < crossing.DistanceFromTrackStart - kilometersBeforeAndAfterCrossing);
                if (indexForSecondInsert == -1)
                    indexForSecondInsert = signsAhead.Count;

                double speedBeforeCrossing = indexForInsert == 0 ? startingPointMaxSpeed : signsAhead[indexForInsert - 1].MaxSpeed;

                signsAhead.Insert(
                indexForSecondInsert,
                new RailroadSign() { IsFacedUp = isMovingUp, MaxSpeed = speedBeforeCrossing, DistanceFromTrackStart = crossing.DistanceFromTrackStart + (isMovingUp ? kilometersBeforeAndAfterCrossing : -1 * kilometersBeforeAndAfterCrossing) }
                );
            });

            authorityContainer.RegisterSpeed(startingPointMaxSpeed, metersSoFar);
            signsAhead.ForEach(s => authorityContainer.RegisterSpeed(
                s.MaxSpeed,
                isMovingUp ? metersSoFar + (s.GetDistanceFromStartMeters() + currentTrack.GetMeter() - currentMeter) : metersSoFar + (currentMeter - s.GetDistanceFromStartMeters() - currentTrack.GetMeter())
            ));
        }

        private void RegisterGradient(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd)
        {
            double gradient = destinationTrackEnd == TrackEnd.RIGHT ? authorityContainer.CurrentTrack!.Gradient : -1 * authorityContainer.CurrentTrack!.Gradient;
            authorityContainer.RegisterGradient(gradient, authorityContainer.MetersSoFar);
        }

        private void UpdateTravelledDistance(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, TrainPosition originalTrainPosition)
        {
            Track currentTrack = authorityContainer.CurrentTrack!;
            if (currentTrack.TrackageElementId == authorityContainer.StartingTrackId)
                authorityContainer.MetersSoFar += (destinationTrackEnd == TrackEnd.RIGHT) ? currentTrack.GetMeter() + currentTrack.GetLengthMeters() - originalTrainPosition.GetMeter() : originalTrainPosition.GetMeter() - currentTrack.GetMeter();
            else authorityContainer.MetersSoFar += currentTrack.GetLengthMeters();
        }

        private void UpdateTravelledDistance(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, TrainPosition originalTrainPosition, RailwaySignal stopSignal)
        {
            bool isMovingUp = destinationTrackEnd == TrackEnd.RIGHT;
            Track currentTrack = authorityContainer.CurrentTrack!;
            if (currentTrack.TrackageElementId == authorityContainer.StartingTrackId)
                authorityContainer.MetersSoFar += isMovingUp ? currentTrack.GetMeter() + stopSignal.GetDistanceFromStartMeters() - originalTrainPosition.GetMeter() : originalTrainPosition.GetMeter() - (stopSignal.GetDistanceFromStartMeters() + currentTrack.GetMeter());
            else authorityContainer.MetersSoFar += isMovingUp ? stopSignal.GetDistanceFromStartMeters() : currentTrack.GetLengthMeters() - stopSignal.GetDistanceFromStartMeters();
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
                authorityContainer.CurrentMeter = track.GetMeter();
                authorityContainer.TrackageElements.Add(track);
            }
            else authorityContainer.CurrentTrack = null;
        }

        private void HandleSwitch(Switch trainSwitch, int trackFromId, MovementAuthorityContainer authorityContainer)
        {
            authorityContainer.TrackageElements.Add(trainSwitch);
            int nextTrackId = switchStates.GetNextTrackId(trainSwitch.TrackageElementId, trackFromId);
            TrackageElement trackageElementAfterSwitch = trackHelper.GetTrackageElement(nextTrackId);

            while (trackageElementAfterSwitch != null && trackageElementAfterSwitch is not Track)
            {
                if (trackageElementAfterSwitch is SwitchingTrack switchingTrack)
                {
                    TrackEnd currentSwitchingTrackEnd = switchingTrack.RightSideElementId == trainSwitch.TrackageElementId ? TrackEnd.RIGHT : TrackEnd.LEFT;
                    RegisterSwitchingTrack(switchingTrack, currentSwitchingTrackEnd, authorityContainer);
                    trainSwitch = (Switch)switchingTrack.GetNext(currentSwitchingTrackEnd.GetOppositeEnd())!;

                    authorityContainer.TrackageElements.Add(switchingTrack);
                    authorityContainer.TrackageElements.Add(trainSwitch);
                    
                    nextTrackId = switchStates.GetNextTrackId(trainSwitch.TrackageElementId, switchingTrack.TrackageElementId);
                    trackageElementAfterSwitch = trackHelper.GetTrackageElement(nextTrackId);
                }
                else throw new Exception("Trackage element after switch is neither a track or a switching track");
            }

            if (trackageElementAfterSwitch is Track track)
            {
                authorityContainer.CurrentTrack = track;
                authorityContainer.CurrentMeter = track.GetMeter();
                authorityContainer.TrackageElements.Add(track);
            } else
            {
                authorityContainer.CurrentTrack = null;
            }
        }


        private void RegisterSwitchingTrack(SwitchingTrack switchingTrack, TrackEnd startingTrackEnd, MovementAuthorityContainer authorityContainer)
        {
            int metersSoFar = authorityContainer.MetersSoFar;
            double gradient = startingTrackEnd == TrackEnd.LEFT ? switchingTrack.Gradient : -1 * switchingTrack.Gradient;

            authorityContainer.RegisterSpeed(switchingTrack.MaxSpeed, metersSoFar);
            authorityContainer.RegisterGradient(gradient, metersSoFar);
            authorityContainer.MetersSoFar += switchingTrack.GetLengthMeters();
        }

        private void UpdateMovementAuthorityUntilStopSignalIsReached(MovementAuthorityContainer authorityContainer, TrackEnd destinationTrackEnd, TrainPosition trainPosition, RailwaySignal stopSignal)
        {
            RegisterSpeedsForCurrentTrack(authorityContainer, destinationTrackEnd, stopSignal);
            RegisterGradient(authorityContainer, destinationTrackEnd);
            authorityContainer.RegisterLine();
            UpdateTravelledDistance(authorityContainer, destinationTrackEnd, trainPosition, stopSignal);
        }

        private MovementAuthority CreateMovementAuthority(MovementAuthorityContainer movementAuthorityContainer, double finishingSpeed)
        {
            MovementAuthority movementAuthority = movementAuthorityContainer.CreateMovementAuthority(finishingSpeed);
            movementAuthorityTracker.SetActiveMovementAuthority(movementAuthorityContainer.Train.TrainId, movementAuthority, movementAuthorityContainer.TrackageElements);
            return movementAuthority;
        }

        class MovementAuthorityContainer(TrainDto train, TrainPosition trainPosition)
        {
            public TrainDto Train { get; set; } = train;
            public TrainPosition TrainPosition { get; set; } = trainPosition;

            public List<double> Speeds { get; set; } = [];
            public List<double> SpeedsDistances { get; set; } = [];
            public List<double> Gradients { get; set; } = [];
            public List<double> GradientsDistances { get; set; } = [];
            public List<int> Lines { get; set; } = [];
            public List<double> LinesDistances { get; set; } = [];

            public int StartingTrackId { get; set; }
            public int CurrentMeter { get; set; }
            public int MetersSoFar { get; set; }
            public Track? CurrentTrack { get; set; }
            public List<TrackageElement> TrackageElements { get; set; } = [];

            public void RegisterSpeed(double speed, int meter)
            {
                if (Speeds.Count > 0 && Speeds.Last() < speed)
                    meter += Train.LengthMeters;

                RegisterSpeedWithMeterComparison(speed, meter);
            }

            public void RegisterSpeedWithMeterComparison(double speed, int meter)
            {
                if (Speeds.Count == 0 || speed != Speeds.Last())
                {
                    if (SpeedsDistances.Count == 0 || meter > SpeedsDistances.Last())
                    {
                        Speeds.Add(speed);
                        SpeedsDistances.Add(meter);
                    }
                    else if (meter < SpeedsDistances.Last())
                    {
                        while (meter < SpeedsDistances.Last())
                        {
                            Speeds.RemoveAt(Speeds.Count - 1);
                            SpeedsDistances.RemoveAt(SpeedsDistances.Count - 1);
                        };
                        RegisterSpeedWithMeterComparison(speed, meter);
                    }
                    else
                    {
                        Speeds[Speeds.Count - 1] = Math.Min(Speeds.Last(), speed);
                    }
                }
            }

            public void RegisterGradient(double gradient, int meter)
            {
                if (Gradients.Count == 0 || gradient != Gradients.Last())
                {
                    Gradients.Add(gradient);
                    GradientsDistances.Add(meter);
                }
            }

            public void RegisterLine()
            {
                if (CurrentTrack != null && (Lines.Count == 0 || CurrentTrack.LineNumber != Lines.Last()))
                {
                    Lines.Add(CurrentTrack.LineNumber);
                    LinesDistances.Add(MetersSoFar);
                }
            }

            public MovementAuthority CreateMovementAuthority(double finishingSpeed)
            {
                RegisterSpeedWithMeterComparison(finishingSpeed, MetersSoFar);
                if (GradientsDistances.Last() == MetersSoFar)
                {
                    Gradients.RemoveAt(Gradients.Count - 1);
                }
                else GradientsDistances.Add(MetersSoFar);

                LinesDistances.Add(MetersSoFar);

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
                    ServerPosition = TrainPosition.Kilometer
                };
            }
        }
    }
}
