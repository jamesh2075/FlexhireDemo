using FlexhireDemo.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;

namespace FlexhireDemo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlexhireController : ControllerBase
    {
        private readonly FlexhireApiKeyProvider _apiKeyProvider;
        private readonly GraphQLService _graphQLService;
        private readonly IHubContext<WebhookHub> _hubContext;

        public FlexhireController(FlexhireApiKeyProvider apiKeyProvider, GraphQLService graphQLService, IHubContext<WebhookHub> hubContext)
        {
            _apiKeyProvider = apiKeyProvider;
            _graphQLService = graphQLService;
            _hubContext = hubContext;
        }

        public class ApiKeyModel
        {
            public string ApiKey { get; set; }
        }

        [HttpPost("apikey")]
        public IActionResult SetApiKey([FromBody] ApiKeyModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ApiKey))
                return BadRequest("API key is required.");

            _apiKeyProvider.ApiKey = model.ApiKey;

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
        public async Task<IActionResult> SimulateWebhook()
        {
            // Trigger a fake webhook callback (usually used for testing)

            FlexhireWebhookPayload fakePayload = new FlexhireWebhookPayload
            {
                EventName = "fake-web-hook",
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Records = [ "dXNlcnMtMTA1OA==" ]
            };
            return await HandleWebhook(fakePayload);
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook([FromBody] FlexhireWebhookPayload payload)
        {
            // Process the incoming webhook event
            await _hubContext.Clients.All.SendAsync("WebhookReceived", payload);

            return Ok();
        }
    }
}


