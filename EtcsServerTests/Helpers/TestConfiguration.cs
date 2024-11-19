using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtcsServerTests.Helpers
{
    public class TestConfiguration
    {
        public IConfiguration Configuration { get; set; }

        List<KeyValuePair<string, string?>>? configuration =
            [
                new("ServerProperties:UnityAppUrl", "test"),
                new("ServerProperties:DriverAppUrl", "test"),
                new("EtcsProperties:MaxSpeedDamagedCrossing", "20"),
                new("EtcsProperties:DamagedCrossingImpactLength", "0.01"),
                new("Security:Base64EncodedAesKey", "DZR+F7EiPSj8qspCCk9DMtMoGq54fEbXLozOiQypZOo="),
                new("Security:Base64EncodedInitialisationVector", "Gis8TV5vcIGSo7TF1uf4CQ==")
            ];

        public TestConfiguration()
        {
            Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configuration)
            .Build();
        }
    }
}
