using Azure.Core;
using EtcsServer.Controllers;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.InMemoryHolders;
using EtcsServer.MapLoading;
using EtcsServer.Security;
using EtcsServer.Senders;
using EtcsServer.Senders.Contracts;
using EtcsServerTests.Helpers;
using EtcsServerTests.TestMaps;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EtcsServer.Controllers.UnityAppController;

namespace EtcsServerTests.Tests
{
    public class TestScenariosForUnityMap
    {
        private UnityMap testMap;
        private ServiceProvider serviceProvider;
        private TestRequestSender testRequestSender;
        private TrainDto Train { get; set; }
        private string crossingMessage;

        public TestScenariosForUnityMap()
        {
            testMap = new UnityMap();
            serviceProvider = testMap.GetTestMapServiceProvider();
            testRequestSender = new(serviceProvider);

            testMap.RailwaySignalStates.SetRailwaySignalState(211, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(15, RailwaySignalMessage.GO);

            this.Train = new()
            {
                TrainId = "97888",
                LengthMeters = 500,
                MaxSpeed = 200,
                BrakeWeight = 1000
            };
            testMap.RegisteredTrainsTracker.Register(Train);
            crossingMessage = new TestConfiguration().Configuration["EtcsProperties:CrossingMessage"];
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnSwitchStateChangeMovingUp()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 2.1,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.SwitchStates.SetSwitchState(121, new SwitchFromTo(211, 11));
            testMap.SwitchStates.SetSwitchState(122, new SwitchFromTo(12, 13));
            testMap.SwitchStates.SetSwitchState(124, new SwitchFromTo(11, 15));
            testMap.SwitchStates.SetSwitchState(123, new SwitchFromTo(13, 16));
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [160, 0],
                SpeedDistances = [0, 6846],
                Gradients = [0],
                GradientsDistances = [0, 6846],
                Lines = [1],
                LinesDistances = [0, 6846],
                Messages = [ crossingMessage ],
                MessagesDistances = [ 3448 ],
                ServerPosition = 2.1
            };
            Assert.Equal(expected, movementAuthority);

            //When
            TrainPosition newPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 2.6,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(newPosition);
            testRequestSender.ImitateReceivingSwitchStateFromUnity(121, false);

            //Then
            MovementAuthority expectedAfterSwitchChange = new()
            {
                Speeds = [160, 60, 160, 0],
                SpeedDistances = [0, 850, 1420, 6366],
                Gradients = [0],
                GradientsDistances = [0, 6366],
                Lines = [1],
                LinesDistances = [0, 6366],
                Messages = [ crossingMessage ],
                MessagesDistances = [ 2968 ],
                ServerPosition = 2.6
            };
            Assert.True(messageToDriver.Timestamp > originalDateTime);
            Assert.Equal(expectedAfterSwitchChange, messageToDriver.MovementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnSwitchStateChangeMovingDown()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 4.1,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.SwitchStates.SetSwitchState(121, new SwitchFromTo(12, 211));
            testMap.SwitchStates.SetSwitchState(121, new SwitchFromTo(11, 211));
            testMap.SwitchStates.SetSwitchState(122, new SwitchFromTo(13, 12));
            testMap.SwitchStates.SetSwitchState(124, new SwitchFromTo(15, 14));
            testMap.SwitchStates.SetSwitchState(123, new SwitchFromTo(14, 13));
            testMap.RailwaySignalStates.SetRailwaySignalState(15, RailwaySignalMessage.GO);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [160, 60, 160, 150, 0],
                SpeedDistances = [0, 450, 1190, 2340, 3640],
                Gradients = [0],
                GradientsDistances = [0, 3640],
                Lines = [1],
                LinesDistances = [0, 3640],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 4.1
            };
            Assert.Equal(expected, movementAuthority);

            //When
            TrainPosition newPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 4.0,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(newPosition);
            testRequestSender.ImitateReceivingSwitchStateFromUnity(124, true);

            //Then
            MovementAuthority expectedAfterSwitchChange = new()
            {
                Speeds = [160, 150, 0],
                SpeedDistances = [0, 2200, 3500],
                Gradients = [0],
                GradientsDistances = [0, 3500],
                Lines = [1],
                LinesDistances = [0, 3500],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 4.0
            };
            Assert.True(messageToDriver.Timestamp > originalDateTime);
            Assert.Equal(expectedAfterSwitchChange, messageToDriver.MovementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnCrossingStateChangeTrainBeforeCrossing()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 5.1,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.SwitchStates.SetSwitchState(131, new SwitchFromTo(15, 17));
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [160, 0],
                SpeedDistances = [0, 3846 ],
                Gradients = [0],
                GradientsDistances = [0, 3846 ],
                Lines = [1],
                LinesDistances = [0, 3846],
                Messages = [ crossingMessage ],
                MessagesDistances = [448],
                ServerPosition = 5.1
            };
            Assert.Equal(expected, movementAuthority);

            //When
            TrainPosition newPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 5.2,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(newPosition);
            testRequestSender.ImitateReceivingCrossingStateFromUnity(1, false);

            //Then
            MovementAuthority expectedAfterSwitchChange = new()
            {
                Speeds = [160, 20, 160, 0],
                SpeedDistances = [0, 1138, 1658, 3746],
                Gradients = [0],
                GradientsDistances = [0, 3746],
                Lines = [1],
                LinesDistances = [0, 3746],
                Messages = [crossingMessage],
                MessagesDistances = [348],
                ServerPosition = 5.2
            };
            Assert.True(messageToDriver.Timestamp > originalDateTime);
            Assert.Equal(expectedAfterSwitchChange, messageToDriver.MovementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnCrossingStateChangeTrainAfterCrossing()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 7.0,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.SwitchStates.SetSwitchState(13, new SwitchFromTo(15, 17));
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [160, 0],
                SpeedDistances = [0, 1946],
                Gradients = [0],
                GradientsDistances = [0, 1946],
                Lines = [1],
                LinesDistances = [0, 1946],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 7.0
            };
            Assert.Equal(expected, movementAuthority);

            //When
            testRequestSender.ImitateReceivingCrossingStateFromUnity(1, false);

            //Then
            Assert.True(messageToDriver.Timestamp == originalDateTime);
            Assert.Null(messageToDriver.MovementAuthority);
            Assert.Equal(expected, serviceProvider.GetRequiredService<IMovementAuthorityTracker>().GetActiveMovementAuthority(trainId));
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnCrossingStateChangeTrainBeforeCrossingMovingDown()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 7.2,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.SwitchStates.SetSwitchState(124, new SwitchFromTo(15, 11));
            testMap.RailwaySignalStates.SetRailwaySignalState(15, RailwaySignalMessage.STOP);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [160, 0],
                SpeedDistances = [0, 3450],
                Gradients = [0],
                GradientsDistances = [0, 3450],
                Lines = [1],
                LinesDistances = [0, 3450],
                Messages = [ crossingMessage ],
                MessagesDistances = [ 52 ],
                ServerPosition = 7.2
            };
            Assert.Equal(expected, movementAuthority);

            //When
            TrainPosition newPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 6.8,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(newPosition);
            testRequestSender.ImitateReceivingCrossingStateFromUnity(1, false);

            //Then
            MovementAuthority expectedAfterSwitchChange = new()
            {
                Speeds = [160, 20, 160, 0],
                SpeedDistances = [0, 442, 962, 3050],
                Gradients = [0],
                GradientsDistances = [0, 3050],
                Lines = [1],
                LinesDistances = [0, 3050],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 6.8
            };
            Assert.True(messageToDriver.Timestamp > originalDateTime);
            Assert.Equal(expectedAfterSwitchChange, messageToDriver.MovementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnCrossingStateChangeTrainAfterCrossingMovingDown()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 6.0,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.SwitchStates.SetSwitchState(124, new SwitchFromTo(15, 11));
            testMap.RailwaySignalStates.SetRailwaySignalState(15, RailwaySignalMessage.STOP);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [160, 0],
                SpeedDistances = [0, 2250],
                Gradients = [0],
                GradientsDistances = [0, 2250],
                Lines = [1],
                LinesDistances = [0, 2250],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 6.0
            };
            Assert.Equal(expected, movementAuthority);

            //When
            testRequestSender.ImitateReceivingCrossingStateFromUnity(1, false);

            //Then
            Assert.True(messageToDriver.Timestamp == originalDateTime);
            Assert.Null(messageToDriver.MovementAuthority);
            Assert.Equal(expected, serviceProvider.GetRequiredService<IMovementAuthorityTracker>().GetActiveMovementAuthority(trainId));
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnRailwaySignalStateChangeExtendMa()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 1.5,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.SwitchStates.SetSwitchState(121, new SwitchFromTo(211, 11));
            testMap.SwitchStates.SetSwitchState(124, new SwitchFromTo(11, 15));
            testMap.SwitchStates.SetSwitchState(131, new SwitchFromTo(15, 17));
            testMap.RailwaySignalStates.SetRailwaySignalState(211, RailwaySignalMessage.STOP);
            testMap.RailwaySignalStates.SetRailwaySignalState(1515, RailwaySignalMessage.STOP);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [150, 160, 0],
                SpeedDistances = [0, 800, 1850],
                Gradients = [0],
                GradientsDistances = [0, 1850],
                Lines = [1],
                LinesDistances = [0, 1850],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 1.5
            };
            Assert.Equal(expected, movementAuthority);

            //When
            testRequestSender.ImitateReceivingRailwaySignalStateFromUnity(211, true);

            //Then
            MovementAuthority newExpected = new()
            {
                Speeds = [150, 160, 0],
                SpeedDistances = [0, 800, 7446],
                Gradients = [0],
                GradientsDistances = [0, 7446],
                Lines = [1],
                LinesDistances = [0, 7446],
                Messages = [ crossingMessage ],
                MessagesDistances = [ 4048 ],
                ServerPosition = 1.5
            };
            Assert.True(messageToDriver.Timestamp > originalDateTime);
            Assert.Equal(newExpected, messageToDriver.MovementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnRailwaySignalStateChangeShortenMa()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 1.5,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            testMap.SwitchStates.SetSwitchState(121, new SwitchFromTo(211, 11));
            testMap.SwitchStates.SetSwitchState(124, new SwitchFromTo(11, 15));
            testMap.SwitchStates.SetSwitchState(131, new SwitchFromTo(15, 17));
            testMap.RailwaySignalStates.SetRailwaySignalState(211, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(1515, RailwaySignalMessage.STOP);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [150, 160, 0],
                SpeedDistances = [0, 800, 7446],
                Gradients = [0],
                GradientsDistances = [0, 7446],
                Lines = [1],
                LinesDistances = [0, 7446],
                Messages = [ crossingMessage ],
                MessagesDistances = [ 4048 ],
                ServerPosition = 1.5
            };
            Assert.Equal(expected, movementAuthority);

            //When
            testRequestSender.ImitateReceivingRailwaySignalStateFromUnity(211, false);

            //Then
            MovementAuthority newExpected = new() {
                Speeds = [150, 160, 0],
                SpeedDistances = [0, 800, 1850],
                Gradients = [0],
                GradientsDistances = [0, 1850],
                Lines = [1],
                LinesDistances = [0, 1850],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 1.5
            };
            Assert.True(messageToDriver.Timestamp > originalDateTime);
            Assert.Equal(newExpected, messageToDriver.MovementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnCrossingFixedTrainBeforeCrossingMovingDown()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 6.8,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.SwitchStates.SetSwitchState(124, new SwitchFromTo(15, 11));
            testMap.CrossingStates.SetCrossingState(1, false);
            testMap.RailwaySignalStates.SetRailwaySignalState(15, RailwaySignalMessage.STOP);
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            PrepareFakeMessageSender(messageToDriver);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [160, 20, 160, 0],
                SpeedDistances = [0, 442, 962, 3050],
                Gradients = [0],
                GradientsDistances = [0, 3050],
                Lines = [1],
                LinesDistances = [0, 3050],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 6.8
            };
            Assert.Equal(expected, movementAuthority);

            //When
            TrainPosition newPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 6.7,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(newPosition);
            testRequestSender.ImitateReceivingCrossingStateFromUnity(1, true);

            //Then
            MovementAuthority expectedAfterSwitchChange = new()
            {
                Speeds = [160, 0],
                SpeedDistances = [0, 2950],
                Gradients = [0],
                GradientsDistances = [0, 2950],
                Lines = [1],
                LinesDistances = [0, 2950],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 6.7
            };
            Assert.True(messageToDriver.Timestamp > originalDateTime);
            Assert.Equal(expectedAfterSwitchChange, messageToDriver.MovementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnLeavingEtcsZone()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 10.466,
                LineNumber = 1,
                Track = "2",
                Direction = "N"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(227229, RailwaySignalMessage.GO);
            testMap.SwitchStates.SetSwitchState(142, new SwitchFromTo(229, 25));
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [60, 120, 80],
                SpeedDistances = [0, 520, 1130],
                Gradients = [0],
                GradientsDistances = [0, 1130],
                Lines = [1],
                LinesDistances = [0, 1130],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 10.466
            };
            Assert.Equal(expected, movementAuthority);
        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnEnteringEtcsZone()
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 12.5,
                LineNumber = 1,
                Track = "1",
                Direction = "P"
            };
            testMap.RailwaySignalStates.SetRailwaySignalState(27, RailwaySignalMessage.GO);
            testMap.SwitchStates.SetSwitchState(144, new SwitchFromTo(243, 26));
            testMap.SwitchStates.SetSwitchState(143, new SwitchFromTo(26, 25));
            testMap.SwitchStates.SetSwitchState(142, new SwitchFromTo(25, 229));
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [80, 40, 80, 120, 60, 40, 140, 0],
                SpeedDistances = [0, 604, 1174, 1424, 2034, 2054, 2574, 3174],
                Gradients = [0],
                GradientsDistances = [0, 3174],
                Lines = [1],
                LinesDistances = [0, 3174],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 12.5
            };
            Assert.Equal(expected, movementAuthority);
        }

        private void PrepareFakeMessageSender(FakeMessageToDriver messageToDriver)
        {
            A.CallTo(() => testRequestSender.DriverAppSender.SendNewMovementAuthority(Train.TrainId, A<MovementAuthority>.Ignored))
                .ReturnsLazily((string trainId, MovementAuthority movementAuthority) =>
                {
                    messageToDriver.Timestamp = DateTime.Now;
                    messageToDriver.MovementAuthority = movementAuthority;
                    return Task.CompletedTask;
                });
        }
    }
}
