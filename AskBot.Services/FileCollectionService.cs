using AskBot.Services.Interfaces;

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
