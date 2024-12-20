﻿using EtcsServer.Controllers;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.Security;
using EtcsServerTests.Helpers;
using EtcsServerTests.TestMaps;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EtcsServerTests.Tests
{
    public class TestScenariosForMap1
    {
        private TestMap1 testMap;
        private ServiceProvider serviceProvider;
        private TestRequestSender testRequestSender;

        public TestScenariosForMap1()
        {
            testMap = new TestMap1();
            serviceProvider = testMap.GetTestMapServiceProvider();
            testRequestSender = new(serviceProvider);

            testMap.SwitchStates.SetSwitchState(4, new SwitchFromTo(3, 6));
            testMap.SwitchStates.SetSwitchState(7, new SwitchFromTo(6, 9));
            testMap.SwitchStates.SetSwitchState(10, new SwitchFromTo(9, 11));
            testMap.SwitchStates.SetSwitchState(12, new SwitchFromTo(11, 15));
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaWhenAllSignalsStop()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 0.5,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [200, 0],
                SpeedDistances = [0, 1500],
                Gradients = [20],
                GradientsDistances = [0, 1500],
                Lines = [1],
                LinesDistances = [0, 1500],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.5
            };
            Assert.Equal(expected, movementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_IncomingFromOutsideOfTheZone()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 0.5,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(12, RailwaySignalMessage.GO);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [200, 180, 0],
                SpeedDistances = [0, 1500, 2500],
                Gradients = [20, 18],
                GradientsDistances = [0, 1500, 2500],
                Lines = [1],
                LinesDistances = [0, 2500],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.5
            };
            Assert.Equal(expected, movementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_IncomingFromIncomingEtcsZoneThroughTwoSignals()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 2.5,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(23, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(3, RailwaySignalMessage.GO);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [180, 70, 0],
                SpeedDistances = [0, 500, 5500],
                Gradients = [18, 7],
                GradientsDistances = [0, 500, 5500],
                Lines = [1],
                LinesDistances = [0, 5500],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 2.5
            };
            Assert.Equal(expected, movementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaThroughSwitchWithStopSignalAtTrainFrontPosition()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 5.5,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(3, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(33, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(4, RailwaySignalMessage.GO);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [70, 30, 0],
                SpeedDistances = [0, 2500, 3500],
                Gradients = [7, 3],
                GradientsDistances = [0, 2500, 3500],
                Lines = [1, 2],
                LinesDistances = [0, 2500, 3500],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 5.5
            };
            Assert.Equal(expected, movementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaForTrainDrivingTheOtherWayRound()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 7,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(33, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(4, RailwaySignalMessage.GO);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [70, 0],
                SpeedDistances = [0, 4000],
                Gradients = [-7],
                GradientsDistances = [0, 4000],
                Lines = [1],
                LinesDistances = [0, 4000],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 7
            };
            Assert.Equal(expected, movementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaToEtcsBorderWithGoSignal()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 0.8,
                LineNumber = 2,
                Track = "1",
                Direction = "N"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(7, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(1516, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(1617, RailwaySignalMessage.GO);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [30, 20, 50, 80],
                SpeedDistances = [0, 200, 1300, 2250],
                Gradients = [3, 2, 5, 6.5],
                GradientsDistances = [0, 200, 500, 1500, 2250],
                Lines = [2],
                LinesDistances = [0, 2250],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.8
            };
            Assert.Equal(expected, movementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaToEtcsBorderWithStopSignal()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 0.8,
                LineNumber = 2,
                Track = "1",
                Direction = "N"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(7, RailwaySignalMessage.GO);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [30, 20, 50, 0],
                SpeedDistances = [0, 200, 1300, 2250],
                Gradients = [3, 2, 5, 6.5],
                GradientsDistances = [0, 200, 500, 1500, 2250],
                Lines = [2],
                LinesDistances = [0, 2250],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.8
            };
            Assert.Equal(expected, movementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaWhenSwitchIsInInvalidState()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 2,
                LineNumber = 2,
                Track = "1",
                Direction = "P"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(77, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(44, RailwaySignalMessage.GO);
            testMap.SwitchStates.SetSwitchState(4, new SwitchFromTo(3, 5));
            testMap.SwitchStates.SetSwitchState(4, new SwitchFromTo(6, 3));
            testMap.SwitchStates.SetSwitchState(7, new SwitchFromTo(6, 9));
            testMap.SwitchStates.SetSwitchState(7, new SwitchFromTo(8, 6));
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [90, 30, 70, 0],
                SpeedDistances = [0, 1000, 2800, 4500],
                Gradients = [-9, -3, -7],
                GradientsDistances = [0, 1000, 2000, 4500],
                Lines = [2, 1],
                LinesDistances = [0, 2000, 4500],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 2
            };
            Assert.Equal(expected, movementAuthority);
        }        
    }
}
