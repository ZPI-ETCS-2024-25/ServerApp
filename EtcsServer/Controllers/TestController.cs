using EtcsServer.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using static EtcsServer.Controllers.TestController;

namespace EtcsServer.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
         private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/")]
        public async Task<ActionResult> PostMessageListener()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                string message = await stream.ReadToEndAsync();
                _logger.LogInformation("Received string content: {}", message);
                return Ok("Server received the message: " + message);
            }
        }

        [HttpGet]
        [Route("heartbeat")]
        public String GetHeartbeat()
        {
            _logger.LogInformation("Heartbeat endpoint reached");
            return DateTime.Now.ToString();
        }

        [HttpGet]
        [Route("pingDriver")]
        public async Task SendMessageToDriverApp(IOptions<ServerProperties> serverProperties)
        {
            _logger.LogInformation("Trying to send message to Driver application");
            string response = await pingComponent(serverProperties.Value.DriverAppUrl);
            _logger.LogInformation("Got message from {}: {}", serverProperties.Value.DriverAppUrl, response);
        }

        [HttpGet]
        [Route("pingUnity")]
        public async Task SendMessageToUnityApp(IOptions<ServerProperties> serverProperties)
        {
            _logger.LogInformation("Trying to send message to Unity application");
            string response = await pingComponent(serverProperties.Value.UnityAppUrl);
            _logger.LogInformation("Got message from {}: {}", serverProperties.Value.UnityAppUrl, response);
        }

        private async Task<string> pingComponent(String componentUrl)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(componentUrl);
            var response = await client.PostAsync("/", new StringContent("Message from the ETCS server"));
            _logger.LogInformation("Response status code from {} is {}", componentUrl, response.StatusCode);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
