using System.Threading.Tasks;

namespace AskBot.Services.Interfaces
{
    public interface IFileCollectionService
    {
        Task GetById(string id);
    }
}
