using System;
using System.IO;
using System.Threading.Tasks;
using Bomberjam.Website.Github;

namespace Bomberjam.Website.Storage
{
    public interface IBomberjamStorage
    {
        Task UploadBotSourceCode(Guid botId, Stream fileStream);
        Task UploadCompiledBot(Guid botId, Stream fileStream);
        Task UploadGameResult(Guid botId, Stream fileStream);

        Task DownloadBotSourceCodeTo(Guid botId, Stream destinationStream);
        Task DownloadCompiledBotTo(Guid botId, Stream destinationStream);
        Task DownloadGameResultTo(Guid botId, Stream destinationStream);

        Task UploadStarter(StarterOs os, Stream fileStream);
        Task DownloadStarter(StarterOs os, Stream destinationStream);
    }
}