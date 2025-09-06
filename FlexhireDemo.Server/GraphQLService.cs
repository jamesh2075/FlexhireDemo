namespace FlexhireDemo.Server
{
    using FlexhireDemo.Server.Models;
    using GraphQL;
    using GraphQL.Client.Http;
    using GraphQL.Client.Serializer.Newtonsoft;
    using Newtonsoft.Json.Linq;
    using System.Net.Http.Headers;

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
