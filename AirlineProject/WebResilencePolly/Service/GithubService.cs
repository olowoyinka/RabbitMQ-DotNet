using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Net;
using WebResilencePolly.Models;

namespace WebResilencePolly.Service
{
    public class GithubService : IGithubService
    {
        private const int MaxRetries = 3;
        private static readonly Random Random = new Random();
        
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AsyncRetryPolicy _retryPolicy;

        public GithubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            //_retryPolicy = Policy.Handle<HttpRequestException>().RetryAsync(retryCount: MaxRetries);
            _retryPolicy = Policy.Handle<HttpRequestException>()
                                .WaitAndRetryAsync(retryCount: MaxRetries, times => TimeSpan.FromMilliseconds(times * 100));
        }

        public async Task<dynamic?> GetUserByUsernameAsync(string username)
        {
            var client = _httpClientFactory.CreateClient(name: "Github");

            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                var result = await client.GetAsync($"users/{username}");

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                string resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<dynamic>(resultString);
            });

            return result;
        }
        
        public async Task<List<GithubUser>?> GetUserFromOrgAsync(string orgName)
        {
            var client = _httpClientFactory.CreateClient(name: "Github");

            var result = await _retryPolicy.ExecuteAsync(async () =>
            {
                var result = await client.GetAsync($"/orgs/{orgName}");

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                string resultString = await result.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<GithubUser>>(resultString);
            });

            return result;
        }
    }
}