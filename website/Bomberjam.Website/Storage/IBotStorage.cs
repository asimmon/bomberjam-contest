using System;
using System.IO;
using System.Threading.Tasks;

namespace Bomberjam.Website.Storage
{
    public interface IBotStorage
    {
        Task UploadBotSourceCode(Guid userId, Stream fileStream);
        Stream DownloadBotSourceCode(Guid userId);

        Task UploadCompiledBot(Guid userId, Stream fileStream);
        Stream DownloadCompiledBot(Guid userId);

        Task UploadGameResult(Guid gameId, Stream fileStream);
        Stream DownloadGameResult(Guid gameId);
    }
}