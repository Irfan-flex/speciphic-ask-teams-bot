using AskBot.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AskBot.Services
{
    public class FileCollectionService : IFileCollectionService
    {
        private readonly IAskUriService _askUriService;
        private readonly HttpClient _httpClient;

        public FileCollectionService(IAskUriService uriService, IHttpClientFactory factory)
        {
            _askUriService = uriService;
            _httpClient = factory.CreateClient("askHttpClient");
        }

        public async Task GetById(string id)
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_askUriService.GetFileCollectionByIdUri(id));
        }
    }
}
