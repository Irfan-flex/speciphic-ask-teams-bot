using AskBot.Services.Interfaces;
using AskBot.Services.Models.In;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AskBot.Services.Search
{
    public class SearchService : ISearchService
    {
        private readonly IAskUriService _askUriService;
        private readonly HttpClient _httpClient;
        public SearchService(IAskUriService askUriService, IHttpClientFactory factory)
        {
            _askUriService = askUriService;
            _httpClient = factory.CreateClient("askHttpClient");
        }

        public async Task<IEnumerable<SearchResponse>> SearchAsync(string fileCollectionId, string query)
        {
            var requestBody = new StringContent(
                new SearchRequestBody
                {
                    AcceptLanguage = "en-US"
                }.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await _httpClient.PostAsync(_askUriService.GetSearchUri(fileCollectionId, query), requestBody);
            return await responseMessage.Content.ReadFromJsonAsync<List<SearchResponse>>(new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
