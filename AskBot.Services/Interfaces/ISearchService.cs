using AskBot.Services.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskBot.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResponse>> SearchAsync(string fileCollectionId, string query);
    }
}
