using Microsoft.Extensions.Configuration;
using System;

namespace AskBot.Services.AskUri
{
    public class AskUriService : IAskUriService
    {
        private readonly string _baseUrl;
        private readonly string _fileCollectionId;
        private readonly string _askApiVersionPrefix;
        public AskUriService(IConfiguration config)
        {
            _baseUrl = config["AskApiBaseUrl"];
            _askApiVersionPrefix = config["AskApiVersionPrefix"];
            _fileCollectionId = config["DefaultFileCollectionId"];
        }

        public string GetFileCollectionByIdUri(string id = null)
        {
            return $"{_askApiVersionPrefix}/file-collections/{id ?? _askApiVersionPrefix}";
        }

        public string GetSearchUri(string fileCollectionId, string query)
        {
            return $"{_askApiVersionPrefix}/file-collections/{fileCollectionId}/search?q={query}";
        }
    }
}
