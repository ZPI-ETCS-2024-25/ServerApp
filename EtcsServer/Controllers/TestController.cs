using EtcsServer.Configuration;
using EtcsServer.Security;
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

        public class JsonResponse
        {
            public string message { get; set; }
        }

        public class PostDATA
        {
            public string name { get; set; }
        }

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/testPost")]
        public async Task<ActionResult> PostMessageListener()
        {
            string message = await getPostBodyAsync();
            _logger.LogInformation("Received string content: {}", message);
            return Ok("Server received the message: " + message);
        }

        [HttpPost]
        [Route("/name")]
        public async Task<ActionResult> PostNameMessageListener(PostDATA postData)
        {
            _logger.LogInformation("Received string content in POST name: {}", postData.name);
            return Ok(new JsonResponse() { message = "Server received the message: " + postData.name});
        }

        [HttpGet]
        [Route("/name")]
        public async Task<ActionResult> GetName()
        {
            _logger.LogInformation("Received GET request for name");
            return Ok(new JsonResponse() { message = "Server name is ETCS Server" });
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

        private async Task<string> getPostBodyAsync()
        {
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                return await stream.ReadToEndAsync();
            }
        }

        [HttpGet]
        [Route("/encrypt")]
        public async Task<ActionResult> Encrypt(string text, [FromServices] ISecurityManager securityManager)
        {
            return Ok(new JsonResponse() { message = securityManager.Encrypt(text) });
        }

        [HttpGet]
        [Route("/decrypt")]
        public async Task<ActionResult> Decrypt(string text, [FromServices] ISecurityManager securityManager)
        {
            return Ok(new JsonResponse() { message = securityManager.Decrypt(text) });
        }
    }
}
