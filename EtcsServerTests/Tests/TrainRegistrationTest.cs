using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData.Contract;
using EtcsServerTests.Helpers;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtcsServerTests.Tests
{
    public class TrainRegistrationTest
    {
        private IRegisteredTrainsTracker registeredTrainsTracker;

        public TrainRegistrationTest()
        {
            registeredTrainsTracker = new TestServiceProvider().GetService<IRegisteredTrainsTracker>();
        }

        [Fact]
        public void IRegisteredTrainsTracker_Register_CorrectTrainRegistration()
        {
            //Given
            TrainDto trainDto = new TrainDto()
            {
                TrainId = "Test",
                LengthMeters = 100,
                MaxSpeed = 50,
                BrakeWeight = 500
            };

            //When
            bool result = registeredTrainsTracker.Register(trainDto);
            List<TrainDto> registeredTrains = registeredTrainsTracker.GetRegisteredTrains();

            //Then
            Assert.True(result);
            Assert.Contains(trainDto, registeredTrains);
        }
    }
}
