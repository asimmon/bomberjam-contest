using System.IO;
using System.Threading.Tasks;

namespace Bomberjam.Website.Storage
{
    public interface IBotStorage
    {
        int GetFileCount();
        Task UploadBotSourceCode(int userId, Stream fileStream);
        Task UploadCompiledBot(int userId, Stream fileStream);
        Stream DownloadBotSourceCode(int userId);
        Stream DownloadCompiledBot(int userId);
    }
}