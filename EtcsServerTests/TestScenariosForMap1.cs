using EtcsServer.Controllers;
using EtcsServer.Database.Entity;
using EtcsServer.DecisionExecutors;
using EtcsServer.DecisionExecutors.Contract;
using EtcsServer.DecisionMakers.Contract;
using EtcsServer.DriverAppDto;
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
            A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(A<int>.Ignored)).Returns(RailwaySignalMessage.STOP);
        }

        [Fact]
        public void MapTest()
        {
            Assert.True(testMap.TrackageElementHolder.GetValues()[3] is Track);
            Assert.True(testMap.TrackageElementHolder.GetValues()[4] is Switch);
            Assert.True(testMap.TrackageElementHolder.GetValues()[9] is SwitchingTrack);
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
            A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(A<string>.That.Matches(s => s.Equals(trainId)))).Returns(trainPosition);

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

        public MovementAuthority? PostValidMovementAuthorityRequest(MovementAuthorityRequest request)
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

        //[Fact]
        //public void IMovementAuthorityProvider_ProvideMovementAuthority_IncomingFromOutsideOfTheZone()
        //{
        //    //Given
        //    string trainId = testMap.Train.TrainId;
        //    TrainPosition trainPosition = new TrainPosition()
        //    {
        //        TrainId = trainId,
        //        Kilometer = 0.5,
        //        LineNumber = 1,
        //        Track = "1",
        //        Direction = "up"
        //    };
        //    A.CallTo(() => testMap.RailwaySignalStates.GetSignalMessage(A<int>.That.Matches(id => id == 12))).Returns(RailwaySignalMessage.GO);
        //    A.CallTo(() => testMap.TrainPositionTracker.GetLastKnownTrainPosition(A<string>.That.Matches(s => s.Equals(trainId)))).Returns(trainPosition);

        //    //When
        //    MovementAuthority? movementAuthority = PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

        //    //Then
        //    MovementAuthority expected = new()
        //    {
        //        Speeds = [200, 0],
        //        SpeedDistances = [0, 1500],
        //        Gradients = [20],
        //        GradientDistances = [0, 1500],
        //        Lines = [1],
        //        LinesDistances = [0, 1500],
        //        Messages = [],
        //        MessageDistances = [],
        //        ServerPosition = 0.5
        //    };
        //    Assert.Equal(expected, movementAuthority);
        //}
    }
}
