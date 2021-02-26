using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Bomberjam.Website.Storage
{
    public abstract class BaseBomberjamStorage : IBomberjamStorage
    {
        public Task UploadBotSourceCode(Guid botId, Stream fileStream) => this.UploadBot(botId, fileStream, isCompiled: false);
        public Task UploadCompiledBot(Guid botId, Stream fileStream) => this.UploadBot(botId, fileStream, isCompiled: true);
        protected abstract Task UploadBot(Guid botId, Stream fileStream, bool isCompiled);
        public abstract Task UploadGameResult(Guid botId, Stream fileStream);

        public Task DownloadBotSourceCodeTo(Guid botId, Stream destinationStream) => this.DownloadBotTo(botId, destinationStream, isCompiled: false);
        public Task DownloadCompiledBotTo(Guid botId, Stream destinationStream) => this.DownloadBotTo(botId, destinationStream, isCompiled: true);
        protected abstract Task DownloadBotTo(Guid botId, Stream destinationStream, bool isCompiled);
        public abstract Task DownloadGameResultTo(Guid botId, Stream destinationStream);

        protected static string MakeBotFileName(Guid userId, bool isCompiled)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:D}-{1}.zip", userId, isCompiled ? 1 : 0);
        }

        protected static string MakeGameFileName(Guid gameId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:D}.json", gameId);
        }
    }
}