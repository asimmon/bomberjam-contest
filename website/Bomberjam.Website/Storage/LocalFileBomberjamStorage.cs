using System;
using System.IO;
using System.Threading.Tasks;
using Bomberjam.Website.Github;

namespace Bomberjam.Website.Storage
{
    public sealed class LocalFileBomberjamStorage : BaseBomberjamStorage
    {
        private const string BomberjamDirName = ".bomberjam";
        private const string BotsDirName = "bots";
        private const string GamesDirName = "games";
        private const string StartersDirName = "starters";

        private readonly string _botsDirPath;
        private readonly string _gamesDirPath;
        private readonly string _startersDirPath;

        public LocalFileBomberjamStorage(string basePath)
        {
            this._botsDirPath = Path.Combine(basePath, BomberjamDirName, BotsDirName);
            this._gamesDirPath = Path.Combine(basePath, BomberjamDirName, GamesDirName);
            this._startersDirPath = Path.Combine(basePath, BomberjamDirName, StartersDirName);

            Directory.CreateDirectory(this._botsDirPath);
            Directory.CreateDirectory(this._gamesDirPath);
            Directory.CreateDirectory(this._startersDirPath);
        }

        protected override async Task UploadBot(Guid botId, Stream fileStream, bool isCompiled)
        {
            await using (fileStream)
            {
                var filePath = Path.Join(this._botsDirPath, MakeBotFileName(botId, isCompiled));
                await using var localFile = File.Open(filePath, FileMode.Create);
                await fileStream.CopyToAsync(localFile).ConfigureAwait(false);
            }
        }

        public override async Task UploadGameResult(Guid botId, Stream fileStream)
        {
            await using (fileStream)
            {
                var filePath = Path.Join(this._gamesDirPath, MakeGameFileName(botId));
                await using var localFile = File.Open(filePath, FileMode.Create);
                await fileStream.CopyToAsync(localFile).ConfigureAwait(false);
            }
        }

        protected override async Task DownloadBotTo(Guid botId, Stream destinationStream, bool isCompiled)
        {
            var filePath = Path.Join(this._botsDirPath, MakeBotFileName(botId, isCompiled));
            await using var sourceStream = File.OpenRead(filePath);
            await sourceStream.CopyToAsync(destinationStream).ConfigureAwait(false);
        }

        public override async Task DownloadGameResultTo(Guid botId, Stream destinationStream)
        {
            var filePath = Path.Join(this._gamesDirPath, MakeGameFileName(botId));
            await using var sourceStream = File.OpenRead(filePath);
            await sourceStream.CopyToAsync(destinationStream).ConfigureAwait(false);
        }

        public override async Task UploadStarter(StarterOs os, Stream fileStream)
        {
            await using (fileStream)
            {
                var filePath = Path.Join(this._startersDirPath, MakeStarterFileName(os));
                await using var localFile = File.Open(filePath, FileMode.Create);
                await fileStream.CopyToAsync(localFile).ConfigureAwait(false);
            }
        }

        public override async Task DownloadStarter(StarterOs os, Stream destinationStream)
        {
            var filePath = Path.Join(this._startersDirPath, MakeStarterFileName(os));
            await using var sourceStream = File.OpenRead(filePath);
            await sourceStream.CopyToAsync(destinationStream).ConfigureAwait(false);
        }
    }
}