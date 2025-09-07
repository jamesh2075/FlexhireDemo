using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FlexhireDemo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlexhireController : ControllerBase
    {
        private readonly FlexhireApiKeyProvider _apiKeyProvider;
        private readonly GraphQLService _graphQLService;

        public FlexhireController(FlexhireApiKeyProvider apiKeyProvider, GraphQLService graphQLService)
        {
            _apiKeyProvider = apiKeyProvider;
            _graphQLService = graphQLService;
        }

        [HttpPost("apikey")]
        public IActionResult SetApiKey([FromBody] ApiKeyModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ApiKey))
                return BadRequest("API key is required.");

            _apiKeyProvider.ApiKey = model.ApiKey;

            // Optionally: store securely, e.g., in-memory cache, secrets manager, etc.
            return Ok();
        }

        [HttpPost("register-webhook")]
        public async Task<IActionResult> RegisterWebhook()
        {
            var webHook = await _graphQLService.RegisterWebhookAsync();

            if (webHook.Enabled)
                return Ok(webHook);
            else
                return BadRequest("The webhook could not be registered.");
        }

        [HttpPost("unregister-webhook")]
        public async Task<IActionResult> UnregisterWebhook([FromBody] string webhookId)
        {
            var webHook = await _graphQLService.UnregisterWebhookAsync(webhookId);

            if (webHook.Enabled)
                return Ok(webHook);
            else
                return BadRequest("The webhook could not be unregistered.");
        }

        [HttpPost("simulate-webhook")]
        public IActionResult SimulateWebhook()
        {
            // Trigger a fake webhook callback (usually used for testing)
            var fakePayload = new
            {
                evt = "candidate.updated",
                data = new { id = 123, name = "John Doe" }
            };

            // You would typically call your own webhook handler:
            return RedirectToAction("HandleWebhook", "Flexhire", new { payload = fakePayload });
        }

        public class ApiKeyModel
        {
            public string ApiKey { get; set; }
        }

        [HttpPost]
        public IActionResult HandleWebhook([FromBody] dynamic payload)
        {
            // Process the incoming webhook event
            Console.WriteLine($"Received webhook: {payload}");

            return Ok();
        }
    }
}


