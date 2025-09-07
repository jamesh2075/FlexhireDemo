namespace FlexhireDemo.Server
{
    using FlexhireDemo.Server.Models;
    using GraphQL;
    using GraphQL.Client.Http;
    using GraphQL.Client.Serializer.Newtonsoft;
    using Newtonsoft.Json.Linq;
    using System.Net.Http.Headers;
    using static FlexhireDemo.Server.Models.CreateWebhookResponse;

    public class GraphQLService
    {
        private readonly IConfiguration _config;
        private readonly FlexhireApiKeyProvider _apiKeyProvider;

        public GraphQLService(IConfiguration config, FlexhireApiKeyProvider apiKeyProvider)
        {
            _config = config;
            _apiKeyProvider = apiKeyProvider;
        }

        private GraphQLHttpClient CreateClient()
        {
            var token = _apiKeyProvider.ApiKey ?? _config["Flexhire:ApiKey"];

            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(_config["Flexhire:ApiUrl"])
            };

            var client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer());

            // Set headers
            client.HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            client.HttpClient.DefaultRequestHeaders.Add("FLEXHIRE-API-KEY", token);

            return client;
        }

        public async Task<CurrentUser> GetCurrentUserDataAsync()
        {
            using var client = CreateClient();

            var request = new GraphQLRequest
            {
                Query = GraphQLQueries.FullQuery
            };

            var response = await client.SendQueryAsync<CurrentUserResponse>(request);
            return response.Data.currentUser;
        }

        public async Task<Webhook> RegisterWebhookAsync()
        {
            var client = CreateClient();

            var query = GraphQLQueries.CreateWebhookQuery;
            var mutation = new GraphQLRequest
            {
                Query = query,
                Variables = new
                {
                    input = new
                    {
                        clientMutationId = Guid.NewGuid().ToString(),
                        enabled = true,
                        url = _config["Flexhire:WebhookUrl"]
                    }
                }
            };

            var response = await client.SendMutationAsync<CreateWebhookResponse>(mutation);

            if (response.Errors != null && response.Errors.Length > 0)
            {
                var errorMessages = string.Join("; ", response.Errors.Select(e => e.Message));
                throw new Exception($"GraphQL errors: {errorMessages}");
            }

            var payload = response.Data?.CreateWebhook;

            if (payload?.Webhook == null)
            {
                throw new Exception("Webhook creation failed or returned null.");
            }
            // TODO: Save the webhook ID that is returned in case it needs to be
            //       deleted or updated.

            return payload.Webhook;
        }

        public async Task<Webhook> UnregisterWebhookAsync(string webhookId)
        {
            var client = CreateClient();

            var query = GraphQLQueries.DeleteWebhookQuery;
            var mutation = new GraphQLRequest
            {
                Query = query,
                Variables = new
                {
                    input = new
                    {
                        clientMutationId = Guid.NewGuid().ToString(),
                        webhookId = webhookId.ToString()
                    }
                }
            };

            var response = await client.SendMutationAsync<DeleteWebhookResponse>(mutation);

            if (response.Errors != null && response.Errors.Length > 0)
            {
                var errorMessages = string.Join("; ", response.Errors.Select(e => e.Message));
                throw new Exception($"GraphQL errors: {errorMessages}");
            }

            var payload = response.Data?.DeleteWebhook;

            if (payload?.Webhook == null)
            {
                throw new Exception("Webhook creation failed or returned null.");
            }
            // TODO: Save the webhook ID that is returned in case it needs to be
            //       deleted or updated.

            return payload.Webhook;
        }
    }

    public class FlexhireApiKeyProvider
    {
        private string _apiKey;

        public string ApiKey
        {
            get => _apiKey;
            set => _apiKey = value;
        }
    }
}
