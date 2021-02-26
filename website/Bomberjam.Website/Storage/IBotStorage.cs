using System;
using System.IO;
using System.Threading.Tasks;

namespace Bomberjam.Website.Storage
{
    public interface IBotStorage
    {
        Task UploadBotSourceCode(Guid botId, Stream fileStream);
        Task<Stream> DownloadBotSourceCode(Guid botId);

        Task UploadCompiledBot(Guid botId, Stream fileStream);
        Task<Stream> DownloadCompiledBot(Guid botId);

        Task UploadGameResult(Guid botId, Stream fileStream);
        Task<Stream> DownloadGameResult(Guid botId);
    }
}