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
    public class UnityScenariosMapEnds
    {
        private UnityMap testMap;
        private ServiceProvider serviceProvider;
        private DriverAppController driverAppController;
        private UnityAppController unityAppController;
        private ISecurityManager securityManager;
        private IDriverAppSender driverAppSender;
        private TrainDto Train { get; set; }

        public UnityScenariosMapEnds()
        {
            testMap = new UnityMap();
            this.Train = new()
            {
                TrainId = "97888",
                LengthMeters = 50,
                MaxSpeed = 180,
                BrakeWeight = 750
            };
            serviceProvider = testMap.GetTestMapServiceProvider();
            driverAppController = serviceProvider.GetRequiredService<DriverAppController>();
            unityAppController = serviceProvider.GetRequiredService<UnityAppController>();
            securityManager = serviceProvider.GetRequiredService<ISecurityManager>();
            driverAppSender = A.Fake<IDriverAppSender>();

            testMap.RailwaySignalStates.SetRailwaySignalState(11, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(22, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(44, RailwaySignalMessage.GO);
            testMap.RegisteredTrainsTracker.Register(Train);
        }

        [Theory, MemberData(nameof(StartingTrackToExpectedMovementAuthority))]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_LeavingTrackageStart(string trackNumber, MovementAuthority expectedMovementAuthority)
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 0.05,
                LineNumber = 1,
                Track = trackNumber,
                Direction = "N"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            Assert.Equal(expectedMovementAuthority, movementAuthority);
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

        public static IEnumerable<object[]> StartingTrackToExpectedMovementAuthority =>
        new List<object[]>
        {
            new object[] { "1", new MovementAuthority()
            {
                Speeds = [120, 150, 160, 0],
                SpeedDistances = [0, 500, 1800, 3300],
                Gradients = [0],
                GradientsDistances = [0, 3300],
                Lines = [1],
                LinesDistances = [0, 3300],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.05
            } },
            new object[] { "2", new MovementAuthority()
            {
                Speeds = [120, 150, 160, 0],
                SpeedDistances = [0, 500, 1800, 3300],
                Gradients = [0],
                GradientsDistances = [0, 3300],
                Lines = [1],
                LinesDistances = [0, 3300],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.05
            } },
            new object[] { "4", new MovementAuthority()
            {
                Speeds = [120, 40, 120, 150, 160, 0],
                SpeedDistances = [0, 120, 240, 540, 1840, 3340],
                Gradients = [0],
                GradientsDistances = [0, 3340],
                Lines = [1],
                LinesDistances = [0, 3340],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.05
            } }
        };
    }
}
