using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace FlexhireDemo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlexhireController : ControllerBase
    {
        private readonly FlexhireApiKeyProvider _apiKeyProvider;

        public FlexhireController(FlexhireApiKeyProvider apiKeyProvider)
        {
            _apiKeyProvider = apiKeyProvider;
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
            // Use _apiKey to call Flexhire's webhook registration endpoint
            // Example pseudocode:
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKeyProvider.ApiKey);

            var payload = new { url = "https://yourapp.com/api/webhook/flexhire" };
            var response = await client.PostAsJsonAsync("https://api.flexhire.com/webhooks", payload);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            return Ok();
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


