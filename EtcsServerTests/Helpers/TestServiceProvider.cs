using EtcsServer.ExtensionMethods;
using FluentAssertions.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EtcsServerTests.Helpers
{
    public class TestServiceProvider
    {
        private ServiceProvider serviceProvider;
        public TestServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(config =>
            {
                config.AddDebug();
                config.AddConsole();
            });
            serviceCollection.AddProjectServices(new TestConfiguration().Configuration);

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public T GetService<T>() where T : class
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }
}
