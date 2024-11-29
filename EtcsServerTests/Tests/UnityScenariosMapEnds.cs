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

namespace EtcsServerTests.Tests
{
    public class UnityScenariosMapEnds
    {
        private static UnityMap testMap;
        private ServiceProvider serviceProvider;
        private TestRequestSender testRequestSender;
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
            testRequestSender = new(serviceProvider);

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
            testMap.RailwaySignalStates.SetRailwaySignalState(11, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(22, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(44, RailwaySignalMessage.GO);

            //When
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            Assert.Equal(expectedMovementAuthority, movementAuthority);
        }

        [Theory, MemberData(nameof(SwitchStatesToExpectedMovementAuthorityOnReturning))]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_ReturningToTrackageStart(string trackNumber, Action prepareSwitches, MovementAuthority expectedMovementAuthority)
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 0.499,
                LineNumber = 1,
                Track = trackNumber,
                Direction = "P"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            prepareSwitches.Invoke();
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            Assert.Equal(expectedMovementAuthority, movementAuthority);
        }

        [Theory, MemberData(nameof(SwitchStatesToExpectedMovementAuthorityOnFinishing))]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_ReachingTrackageEnd(string trackNumber, Action prepareSwitches, MovementAuthority expectedMovementAuthority)
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 13.65,
                LineNumber = 1,
                Track = trackNumber,
                Direction = "N"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);

            //When
            prepareSwitches.Invoke();
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            Assert.Equal(expectedMovementAuthority, movementAuthority);
        }

        [Theory, MemberData(nameof(SwitchStatesToExpectedMovementAuthorityOnStartingFromFinish))]
        public void IMovementAuthorityProvider_ProvideMovementAuthority_LeavingFromTrackageEnd(string trackNumber, Action prepareSwitches, MovementAuthority expectedMovementAuthority)
        {
            //Given
            string trainId = Train.TrainId;
            TrainPosition trainPosition = new TrainPosition()
            {
                TrainId = trainId,
                Kilometer = 14.296,
                LineNumber = 1,
                Track = trackNumber,
                Direction = "P"
            };
            testMap.TrainPositionTracker.RegisterTrainPosition(trainPosition);
            testMap.RailwaySignalStates.SetRailwaySignalState(3535, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(3636, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(3737, RailwaySignalMessage.GO);
            testMap.RailwaySignalStates.SetRailwaySignalState(3838, RailwaySignalMessage.GO);

            //When
            prepareSwitches.Invoke();
            MovementAuthority? movementAuthority = testRequestSender.PostValidMovementAuthorityRequest(new MovementAuthorityRequest() { TrainId = trainId });

            //Then
            Assert.Equal(expectedMovementAuthority, movementAuthority);
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

        public static IEnumerable<object[]> SwitchStatesToExpectedMovementAuthorityOnReturning =>
        new List<object[]>
        {
            new object[] { "1", () => { }, new MovementAuthority()
            {
                Speeds = [40, 0],
                SpeedDistances = [0, 499],
                Gradients = [0],
                GradientsDistances = [0, 499],
                Lines = [1],
                LinesDistances = [0, 499],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.499
            } },
            new object[] {
                "1", () => {
                    testMap.SwitchStates.SetSwitchState(115, false);
                }, new MovementAuthority()
                {
                    Speeds = [40, 0],
                    SpeedDistances = [0, 519],
                    Gradients = [0],
                    GradientsDistances = [0, 519],
                    Lines = [1],
                    LinesDistances = [0, 519],
                    Messages = [],
                    MessagesDistances = [],
                    ServerPosition = 0.499
                }
            },
            new object[] {
                "2", () => {
                    testMap.SwitchStates.SetSwitchState(112, false);
                },
                new MovementAuthority()
                {
                    Speeds = [40, 0],
                    SpeedDistances = [0, 539],
                    Gradients = [0],
                    GradientsDistances = [0, 539],
                    Lines = [1],
                    LinesDistances = [0, 539],
                    Messages = [],
                    MessagesDistances = [],
                    ServerPosition = 0.499
                }
            }
        };

        public static IEnumerable<object[]> SwitchStatesToExpectedMovementAuthorityOnFinishing =>
        new List<object[]>
        {
            new object[] { "2", () => {
                    testMap.SwitchStates.SetSwitchState(153, false);
            }, new MovementAuthority()
            {
                Speeds = [40, 0],
                SpeedDistances = [0, 716],
                Gradients = [0],
                GradientsDistances = [0, 716],
                Lines = [1],
                LinesDistances = [0, 716],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 13.65
            } },
            new object[] {
                "2", () => { }, new MovementAuthority()
                {
                    Speeds = [40, 0],
                    SpeedDistances = [0, 696],
                    Gradients = [0],
                    GradientsDistances = [0, 696],
                    Lines = [1],
                    LinesDistances = [0, 696],
                    Messages = [],
                    MessagesDistances = [],
                    ServerPosition = 13.65
                }
            },
            new object[] {
                "1", () => {
                    testMap.SwitchStates.SetSwitchState(155, false);
                },
                new MovementAuthority()
                {
                    Speeds = [40, 0],
                    SpeedDistances = [0, 716],
                    Gradients = [0],
                    GradientsDistances = [0, 716],
                    Lines = [1],
                    LinesDistances = [0, 716],
                    Messages = [],
                    MessagesDistances = [],
                    ServerPosition = 13.65
                }
            },
            new object[] {
                "1", () => {
                    testMap.SwitchStates.SetSwitchState(151, false);
                    testMap.SwitchStates.SetSwitchState(156, false);
                },
                new MovementAuthority()
                {
                    Speeds = [40, 0],
                    SpeedDistances = [0, 736],
                    Gradients = [0],
                    GradientsDistances = [0, 736],
                    Lines = [1],
                    LinesDistances = [0, 736],
                    Messages = [],
                    MessagesDistances = [],
                    ServerPosition = 13.65
                }
            }
        };

        public static IEnumerable<object[]> SwitchStatesToExpectedMovementAuthorityOnStartingFromFinish =>
        new List<object[]>
        {
            new object[] { "1", () => { }, new MovementAuthority()
            {
                Speeds = [80, 0],
                SpeedDistances = [0, 2300],
                Gradients = [0],
                GradientsDistances = [0, 2300],
                Lines = [1],
                LinesDistances = [0, 2300],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 14.296
            } },
            new object[] {
                "2", () => {
                    testMap.SwitchStates.SetSwitchState(152, false);
                }, new MovementAuthority()
                {
                    Speeds = [80, 40, 80, 0],
                    SpeedDistances = [0, 450, 570, 2320],
                    Gradients = [0],
                    GradientsDistances = [0, 2320],
                    Lines = [1],
                    LinesDistances = [0, 2320],
                    Messages = [],
                    MessagesDistances = [],
                    ServerPosition = 14.296
                }
            },
            new object[] {
                "3", () => {
                    testMap.SwitchStates.SetSwitchState(154, false);
                },
                new MovementAuthority()
                {
                    Speeds = [80, 40, 80, 40, 80, 0],
                    SpeedDistances = [0, 150, 270, 320, 440, 2340],
                    Gradients = [0],
                    GradientsDistances = [0, 2340],
                    Lines = [1],
                    LinesDistances = [0, 2340],
                    Messages = [],
                    MessagesDistances = [],
                    ServerPosition = 14.296
                }
            },
            new object[] {
                "4", () => { },
                new MovementAuthority()
                {
                    Speeds = [80, 40, 80, 0],
                    SpeedDistances = [0, 150, 270, 2320],
                    Gradients = [0],
                    GradientsDistances = [0, 2320],
                    Lines = [1],
                    LinesDistances = [0, 2320],
                    Messages = [],
                    MessagesDistances = [],
                    ServerPosition = 14.296
                }
            }
        };
    }
}
