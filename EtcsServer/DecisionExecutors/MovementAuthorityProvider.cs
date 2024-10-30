﻿using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors;
using EtcsServer.Helpers;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryHolders;
using System.Web.Http.Tracing;

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
            //TODO include switches in max speeds
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            bool isMovingUp = trainPosition.Direction.Equals("up");

            List<double> _maxSpeeds = [];
            List<double> _maxSpeedsDistances = [];
            List<double> _gradients = [];
            List<double> _gradientsDistances = [];

            double distanceSoFar = 0;

            Track currentTrack = trackHelper.GetTrackByTrainPosition(trainPosition)!;
            int startingTrackId = currentTrack.TrackageElementId;
            while (currentTrack != null && currentTrack.TrackPosition == TrackPosition.INSIDE_ZONE)
            {
                double currentTrackMaxSpeed = isMovingUp ? currentTrack.MaxUpSpeedMps : currentTrack.MaxDownSpeedMps;
                List<RailroadSign> currentSigns = railroadSignsHolder.GetValues().Values
                    .Where(s => s.TrackId == currentTrack.TrackageElementId)
                    .Where(s => s.IsFacedUp = trainPosition.Direction.Equals("up"))
                    .Where(s => s.MaxSpeed < currentTrackMaxSpeed)
                    .OrderBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                    .ToList();

                if (currentSigns.Count > 0)
                {
                    if ((isMovingUp && currentSigns.First().DistanceFromTrackStart > 0) || currentSigns.First().DistanceFromTrackStart < currentTrack.Length)
                    {
                        if (currentTrackMaxSpeed != _maxSpeeds.LastOrDefault()) {
                            _maxSpeeds.Add(currentTrackMaxSpeed);
                            _maxSpeedsDistances.Add(distanceSoFar);
                        }
                    }
                    
                    _maxSpeeds.AddRange(currentSigns.Select(s => s.MaxSpeed));
                    _maxSpeedsDistances.AddRange(currentSigns.Select(s => isMovingUp ? distanceSoFar + s.DistanceFromTrackStart : distanceSoFar + (currentTrack.Length - s.DistanceFromTrackStart)));
                }
                else if (currentTrackMaxSpeed != _maxSpeeds.LastOrDefault())
                {
                    _maxSpeeds.Add(currentTrackMaxSpeed);
                    _maxSpeedsDistances.Add(distanceSoFar);
                }

                if (currentTrack.Gradient !=  _gradients.LastOrDefault())
                {
                    _gradients.Add(currentTrack.Gradient);
                    _gradientsDistances.Add(distanceSoFar);
                }

                if (currentTrack.TrackageElementId == startingTrackId)
                    distanceSoFar += isMovingUp ? currentTrack.Length - trainPosition.Kilometer : trainPosition.Kilometer;
                else distanceSoFar += currentTrack.Length;

                TrackageElement? nextTrackageElement = trackHelper.GetNextTrackageElement(currentTrack.TrackageElementId, isMovingUp);
                if (nextTrackageElement is Switch trainSwitch)
                {
                    double? switchMaxSpeed = switchStates.GetMaxSpeed(trainSwitch.TrackageElementId, currentTrack.TrackageElementId);

                    double maxSpeedBeforeSwitch = _maxSpeeds.Last();
                    if (switchMaxSpeed.HasValue && switchMaxSpeed.Value != maxSpeedBeforeSwitch)
                    {
                        _maxSpeeds.Add(switchMaxSpeed.Value);
                        _maxSpeedsDistances.Add(distanceSoFar);
                        _maxSpeeds.Add(maxSpeedBeforeSwitch);
                        _maxSpeedsDistances.Add(distanceSoFar);
                    }

                    int nextTrackId = switchStates.GetNextTrackId(trainSwitch.TrackageElementId, currentTrack.TrackageElementId);
                    currentTrack = trackHelper.GetTrackById(nextTrackId)!;
                }
                else if (nextTrackageElement is Track track)
                    currentTrack = track;
            }

            return new MovementAuthority()
            {
                Speeds = _maxSpeeds.Append(0).ToArray(),
                SpeedDistances = _maxSpeedsDistances.Append(distanceSoFar).ToArray(),
                Gradients = _gradients.ToArray(),
                GradientDistances = _gradientsDistances.ToArray(),
                Messages = ["Travel safe"],
                MessageDistances = [0],
                ServerPosition = trainPosition.Kilometer //TODO kilometer without track number and line number is ambiguous
            };
        }

        public MovementAuthority ProvideMovementAuthority(string trainId, RailwaySignal stopSignal)
        {
            TrainPosition trainPosition = lastKnownPositionsTracker.GetLastKnownTrainPosition(trainId)!;
            bool isMovingUp = trainPosition.Direction.Equals("up");

            List<double> _maxSpeeds = [];
            List<double> _maxSpeedsDistances = [];
            List<double> _gradients = [];
            List<double> _gradientsDistances = [];

            double distanceSoFar = 0;

            Track currentTrack = trackHelper.GetTrackByTrainPosition(trainPosition)!;
            int startingTrackId = currentTrack.TrackageElementId;
            while (currentTrack != null && currentTrack.TrackPosition == TrackPosition.INSIDE_ZONE)
            {
                //double maxDistance = currentTrack.TrackageElementId == stopSignal.Track.TrackageElementId ? stopSignal.DistanceFromTrackStart : currentTrack.Length;
                double currentTrackMaxSpeed = isMovingUp ? currentTrack.MaxUpSpeedMps : currentTrack.MaxDownSpeedMps;
                List<RailroadSign> currentSigns = railroadSignsHolder.GetValues().Values
                    .Where(s => s.TrackId == currentTrack.TrackageElementId)
                    .Where(s => s.IsFacedUp = trainPosition.Direction.Equals("up"))
                    .Where(s => s.MaxSpeed < currentTrackMaxSpeed)
                    .OrderBy(s => isMovingUp ? s.DistanceFromTrackStart : -1 * s.DistanceFromTrackStart)
                    .ToList();

                if (currentSigns.Count > 0)
                {
                    if ((isMovingUp && currentSigns.First().DistanceFromTrackStart > 0) || currentSigns.First().DistanceFromTrackStart < currentTrack.Length)
                    {
                        if (currentTrackMaxSpeed != _maxSpeeds.LastOrDefault())
                        {
                            _maxSpeeds.Add(currentTrackMaxSpeed);
                            _maxSpeedsDistances.Add(distanceSoFar);
                        }
                    }

                    _maxSpeeds.AddRange(currentSigns.Select(s => s.MaxSpeed));
                    _maxSpeedsDistances.AddRange(currentSigns.Select(s => isMovingUp ? distanceSoFar + s.DistanceFromTrackStart : distanceSoFar + (currentTrack.Length - s.DistanceFromTrackStart)));
                }
                else if (currentTrackMaxSpeed != _maxSpeeds.LastOrDefault())
                {
                    _maxSpeeds.Add(currentTrackMaxSpeed);
                    _maxSpeedsDistances.Add(distanceSoFar);
                }

                if (currentTrack.Gradient != _gradients.LastOrDefault())
                {
                    _gradients.Add(currentTrack.Gradient);
                    _gradientsDistances.Add(distanceSoFar);
                }

                if (currentTrack.TrackageElementId == startingTrackId)
                    distanceSoFar += isMovingUp ? currentTrack.Length - trainPosition.Kilometer : trainPosition.Kilometer;
                else distanceSoFar += currentTrack.Length;
                currentTrack = trackHelper.GetNextTrack(currentTrack.TrackageElementId, isMovingUp)!;
            }

            return new MovementAuthority()
            {
                Speeds = _maxSpeeds.Append(0).ToArray(),
                SpeedDistances = _maxSpeedsDistances.Append(distanceSoFar).ToArray(),
                Gradients = _gradients.ToArray(),
                GradientDistances = _gradientsDistances.ToArray(),
                Messages = ["Travel safe"],
                MessageDistances = [0],
                ServerPosition = trainPosition.Kilometer //TODO kilometer without track number and line number is ambiguous
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
