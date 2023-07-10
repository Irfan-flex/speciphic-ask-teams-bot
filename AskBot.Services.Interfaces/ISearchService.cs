using AskBot.Services.Models.Out;

namespace AskBot.Services.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResponse>> SearchAsync(string fileCollectionId, string query);
    }
}
