using System.Threading.Tasks;

namespace AskBot.Services
{
    public interface IFileCollectionService
    {
        Task GetById(string id);
    }
}
