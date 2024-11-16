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
using EtcsServer.Security;
using EtcsServer.Senders;
using EtcsServer.Senders.Contracts;
using EtcsServerTests.TestMaps;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtcsServerTests.Tests
{
    public class TestScenariosForUnityMap
    {
        private UnityMap testMap;
        private ServiceProvider serviceProvider;
        private DriverAppController driverAppController;
        private UnityAppController unityAppController;
        private ISecurityManager securityManager;
        private IDriverAppSender driverAppSender;

        public TestScenariosForUnityMap()
        {
            testMap = new UnityMap();
            serviceProvider = testMap.GetTestMapServiceProvider();
            driverAppController = serviceProvider.GetRequiredService<DriverAppController>();
            unityAppController = serviceProvider.GetRequiredService<UnityAppController>();
            securityManager = serviceProvider.GetRequiredService<ISecurityManager>();
            driverAppSender = A.Fake<IDriverAppSender>();

            A.CallTo(() => testMap.TrainPositionTracker.GetMovementDirection(testMap.Train.TrainId)).Returns(MovementDirection.UP);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(A<int>.Ignored)).Returns(RailwaySignalMessage.STOP);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(211)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(15)).Returns(RailwaySignalMessage.GO);
            A.CallTo(() => testMap.SwitchStates.SetSwitchState(A<int>.Ignored, A<SwitchFromTo>.Ignored)).Invokes((int switchId, SwitchFromTo newSwitchState) =>
            {
                A.CallTo(() => testMap.SwitchStates.GetNextTrackId(switchId, newSwitchState.TrackIdFrom)).Returns(newSwitchState.TrackIdTo);
            });

        }

        [Fact]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_MaOnSwitchStateChangeMovingUp()
        {
            //Given
            string trainId = testMap.Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 2.1,
                LineNumber = 1,
                Track = "1",
                Direction = "N"
            };
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(trainPosition);

            A.CallTo(() => testMap.SwitchStates.GetNextTrackId(121, 211)).Returns(11);
            A.CallTo(() => testMap.SwitchStates.GetNextTrackId(122, 12)).Returns(13);
            A.CallTo(() => testMap.SwitchStates.GetNextTrackId(124, 11)).Returns(15);
            A.CallTo(() => testMap.SwitchStates.GetNextTrackId(123, 13)).Returns(16);

            FakeMessageToDriver messageToDriver = new();
            DateTime originalDateTime = messageToDriver.Timestamp;
            A.CallTo(() => driverAppSender.SendNewMovementAuthority(trainId, A<MovementAuthority>.Ignored))
                .ReturnsLazily((string trainId, MovementAuthority movementAuthority) =>
                {
                    messageToDriver.Timestamp = DateTime.Now;
                    messageToDriver.MovementAuthority = movementAuthority;
                    return Task.CompletedTask;
                });

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            MovementAuthority expected = new()
            {
                Speeds = [160, 0],
                SpeedDistances = [0, 1650],
                Gradients = [0],
                GradientDistances = [0, 1650],
                Lines = [1],
                LinesDistances = [0, 1650],
                Messages = [],
                MessageDistances = [],
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
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(trainId)).Returns(newPosition);
            ImitateReceivingSwitchStateFromUnity(121, 211, 12);

            //Then
            MovementAuthority expectedAfterSwitchChange = new()
            {
                Speeds = [160, 60, 0],
                SpeedDistances = [0, 850, 1170],
                Gradients = [0],
                GradientDistances = [0, 1170],
                Lines = [1],
                LinesDistances = [0, 1170],
                Messages = [],
                MessageDistances = [],
                ServerPosition = 2.6
            };
            Assert.True(messageToDriver.Timestamp > originalDateTime);
            Assert.Equal(expectedAfterSwitchChange, messageToDriver.MovementAuthority);
        }

        private void ImitateReceivingSwitchStateFromUnity(int switchId, int trackFromId, int trackToId)
        {
            ActionResult response = unityAppController.ChangeSwitchState(
                switchId, trackFromId, trackToId,
                serviceProvider.GetRequiredService<IMovementAuthorityValidator>(),
                serviceProvider.GetRequiredService<IMovementAuthorityProvider>(),
                driverAppSender
            ).Result;

            Assert.True(response is OkResult);
        }

        private MovementAuthority? PostValidMovementAuthorityRequest(MovementAuthorityRequest request)
        {
            ActionResult response = driverAppController.PostMovementAuthorityRequest(
                new EncryptedMessage() { Content = securityManager.Encrypt(request) },
                serviceProvider.GetRequiredService<IMovementAuthorityValidator>(),
                serviceProvider.GetRequiredService<IMovementAuthorityProvider>(),
                serviceProvider.GetRequiredService<IMovementAuthorityTracker>()
            ).Result;

            if (response is ObjectResult objectResult && objectResult.Value is string encryptedResponse)
            {
                if (encryptedResponse != null)
                    return securityManager.Decrypt<MovementAuthority>(encryptedResponse);
                return null;
            }
            return null;
        }

        class FakeMessageToDriver
        {
            public DateTime Timestamp { get; set; } = DateTime.Now;
            public MovementAuthority? MovementAuthority { get; set; }
        }
    }
}
