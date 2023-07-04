using System;

namespace AskBot.Services.Interfaces
{
    public interface IAskUriService
    {
        string GetFileCollectionByIdUri(string id = null);
        string GetSearchUri(string fileCollectionId, string query);
    }
}
