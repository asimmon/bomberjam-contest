using System;
using System.IO;
using System.Threading.Tasks;

namespace Bomberjam.Website.Storage
{
    public sealed class LocalFileBotStorage : BaseBotStorage, IBotStorage
    {
        private const string BomberjamDirName = "Bomberjam";
        private const string BotsDirName = "Bots";
        private const string GamesDirName = "Games";

        private readonly string _botsDirPath;
        private readonly string _gamesDirPath;

        public LocalFileBotStorage(string basePath)
        {
            this._botsDirPath = Path.Combine(basePath, BomberjamDirName, BotsDirName);
            this._gamesDirPath = Path.Combine(basePath, BomberjamDirName, GamesDirName);

            Directory.CreateDirectory(this._botsDirPath);
            Directory.CreateDirectory(this._gamesDirPath);
        }

        public Task UploadBotSourceCode(Guid botId, Stream fileStream)
        {
            return this.UploadBot(botId, false, fileStream);
        }

        public Task UploadCompiledBot(Guid botId, Stream fileStream)
        {
            return this.UploadBot(botId, true, fileStream);
        }

        private async Task UploadBot(Guid botId, bool isCompiled, Stream fileStream)
        {
            await using (fileStream)
            {
                var filePath = Path.Join(this._botsDirPath, MakeBotFileName(botId, isCompiled));
                await using var localFile = File.Open(filePath, FileMode.Create);
                await fileStream.CopyToAsync(localFile).ConfigureAwait(false);
            }
        }

        public Task<Stream> DownloadBotSourceCode(Guid botId)
        {
            return this.DownloadBot(botId, false);
        }

        public Task<Stream> DownloadCompiledBot(Guid botId)
        {
            return this.DownloadBot(botId, true);
        }

        private Task<Stream> DownloadBot(Guid botId, bool isCompiled)
        {
            var filePath = Path.Join(this._botsDirPath, MakeBotFileName(botId, isCompiled));
            return Task.FromResult<Stream>(File.OpenRead(filePath));
        }

        public async Task UploadGameResult(Guid botId, Stream fileStream)
        {
            await using (fileStream)
            {
                var filePath = Path.Join(this._gamesDirPath, MakeGameFileName(botId));
                await using var localFile = File.Open(filePath, FileMode.Create);
                await fileStream.CopyToAsync(localFile).ConfigureAwait(false);;
            }
        }

        public Task<Stream> DownloadGameResult(Guid botId)
        {
            var filePath = Path.Join(this._gamesDirPath, MakeGameFileName(botId));
            return Task.FromResult<Stream>(File.OpenRead(filePath));
        }
    }
}