using EtcsServer.Controllers;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
using EtcsServer.DriverDataCollectors.Contract;
using EtcsServer.InMemoryData;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.Security;
using EtcsServerTests.TestMaps;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtcsServerTests
{
    public class TestScenariosForMap1
    {
        private TestMap1 testMap;
        private ServiceProvider serviceProvider;
        private DriverAppController driverAppController;
        private SecurityManager securityManager;

        public TestScenariosForMap1()
        {
            testMap = new TestMap1();
            serviceProvider = testMap.GetTestMapServiceProvider();
            driverAppController = serviceProvider.GetRequiredService<DriverAppController>();
            securityManager = serviceProvider.GetRequiredService<SecurityManager>();

            A.CallTo(() => testMap.TrainPositionTracker.GetMovementDirection(testMap.Train.TrainId)).Returns(MovementDirection.UP);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(A<int>.Ignored)).Returns(RailwaySignalMessage.STOP);
            A.CallTo(() => testMap.SwitchStates.GetNextTrackId(4, 3)).Returns(6);
            A.CallTo(() => testMap.SwitchStates.GetNextTrackId(7, 6)).Returns(9);
            A.CallTo(() => testMap.SwitchStates.GetNextTrackId(12, 11)).Returns(15);
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
                Direction = "up"
            };
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(trainPosition);

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [200, 0],
                SpeedDistances = [0, 1500],
                Gradients = [20],
                GradientDistances = [0, 1500],
                Lines = [1],
                LinesDistances = [0, 1500],
                Messages = [],
                MessageDistances = [],
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
                Direction = "up"
            };
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(12)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(trainPosition);

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [200, 180, 0],
                SpeedDistances = [0, 1500, 2500],
                Gradients = [20, 18],
                GradientDistances = [0, 1500, 2500],
                Lines = [1],
                LinesDistances = [0, 2500],
                Messages = [],
                MessageDistances = [],
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
                Direction = "up"
            };
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(23)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(3)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(trainPosition);

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [180, 70, 0],
                SpeedDistances = [0, 500, 5500],
                Gradients = [18, 7],
                GradientDistances = [0, 500, 5500],
                Lines = [1],
                LinesDistances = [0, 5500],
                Messages = [],
                MessageDistances = [],
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
                Direction = "up"
            };
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(3)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(33)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(4)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(trainPosition);

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [70, 30, 0],
                SpeedDistances = [0, 2500, 3500],
                Gradients = [7, 3],
                GradientDistances = [0, 2500, 3500],
                Lines = [1, 2],
                LinesDistances = [0, 2500, 3500],
                Messages = [],
                MessageDistances = [],
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
                Direction = "down"
            };
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(33)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(4)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(trainPosition);
            A.CallTo(() => testMap.TrainPositionTracker.GetMovementDirection(trainId)).Returns(MovementDirection.DOWN);

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [70, 0],
                SpeedDistances = [0, 4000],
                Gradients = [ -7 ],
                GradientDistances = [0, 4000],
                Lines = [1],
                LinesDistances = [0, 4000],
                Messages = [],
                MessageDistances = [],
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
                Direction = "up"
            };
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(7)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(1516)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(1617)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(trainPosition);

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [30, 20, 50, 80],
                SpeedDistances = [0, 200, 1300, 2250],
                Gradients = [3, 2, 5, 6.5],
                GradientDistances = [0, 200, 500, 1500, 2250],
                Lines = [2],
                LinesDistances = [0, 2250],
                Messages = [],
                MessageDistances = [],
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
                Direction = "up"
            };
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(7)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(trainPosition);

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [30, 20, 50, 0],
                SpeedDistances = [0, 200, 1300, 2250],
                Gradients = [3, 2, 5, 6.5],
                GradientDistances = [0, 200, 500, 1500, 2250],
                Lines = [2],
                LinesDistances = [0, 2250],
                Messages = [],
                MessageDistances = [],
                ServerPosition = 0.8
            };
            Assert.Equal(expected, movementAuthority);
        }

        private MovementAuthority? PostValidMovementAuthorityRequest(MovementAuthorityRequest request)
        {
            ActionResult response = driverAppController.PostMovementAuthorityRequest(
                request,
                serviceProvider.GetRequiredService<IMovementAuthorityValidator>(),
                serviceProvider.GetRequiredService<IMovementAuthorityProvider>()
            ).Result;

            if (response is OkObjectResult okObjectResult)
            {
                var encryptedResponse = okObjectResult.Value as EncryptedResponse;
                if (encryptedResponse != null)
                    return securityManager.Decrypt<MovementAuthority>(encryptedResponse.EncryptedContent);
                return null;
            }
            return null;
        }
    }
}
