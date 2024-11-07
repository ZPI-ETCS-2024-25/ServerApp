using EtcsServer.Database.Entity;
using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData.Contract;
using EtcsServerTests.TestMaps;
using FakeItEasy;
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

        public TestScenariosForMap1()
        {
            testMap = new TestMap1();
        }

        [Fact]
        public void MapTest()
        {
            Assert.True(testMap.TrackageElementHolder.GetValues()[3] is Track);
            Assert.True(testMap.TrackageElementHolder.GetValues()[4] is Switch);
            Assert.True(testMap.TrackageElementHolder.GetValues()[9] is SwitchingTrack);
        }
    }
}
