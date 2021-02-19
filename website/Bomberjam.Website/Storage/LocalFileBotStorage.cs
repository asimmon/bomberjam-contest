using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Bomberjam.Website.Storage
{
    public sealed class LocalFileBotStorage : IBotStorage
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
            return this.UploadBotSourceCode(botId, false, fileStream);
        }

        public Task UploadCompiledBot(Guid botId, Stream fileStream)
        {
            return this.UploadBotSourceCode(botId, true, fileStream);
        }

        private async Task UploadBotSourceCode(Guid botId, bool isCompiled, Stream fileStream)
        {
            await using (fileStream)
            {
                var filePath = Path.Join(this._botsDirPath, MakeBotFileName(botId, isCompiled));
                await using var localFile = File.Open(filePath, FileMode.Create);
                await fileStream.CopyToAsync(localFile);
            }
        }

        private static string MakeBotFileName(Guid userId, bool isCompiled)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}-{1}.zip", userId.ToString("D"), isCompiled ? 1 : 0);
        }

        public Stream DownloadBotSourceCode(Guid botId)
        {
            return this.DownloadBotSourceCode(botId, false);
        }

        public Stream DownloadCompiledBot(Guid botId)
        {
            return this.DownloadBotSourceCode(botId, true);
        }

        private Stream DownloadBotSourceCode(Guid botId, bool isCompiled)
        {
            var filePath = Path.Join(this._botsDirPath, MakeBotFileName(botId, isCompiled));
            return File.OpenRead(filePath);
        }

        public async Task UploadGameResult(Guid botId, Stream fileStream)
        {
            await using (fileStream)
            {
                var filePath = Path.Join(this._gamesDirPath, MakeGameFileName(botId));
                await using var localFile = File.Open(filePath, FileMode.Create);
                await fileStream.CopyToAsync(localFile);
            }
        }

        public Stream DownloadGameResult(Guid botId)
        {
            var filePath = Path.Join(this._gamesDirPath, MakeGameFileName(botId));
            return File.OpenRead(filePath);
        }

        private static string MakeGameFileName(Guid gameId)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.json", gameId.ToString("D"));
        }
    }
}