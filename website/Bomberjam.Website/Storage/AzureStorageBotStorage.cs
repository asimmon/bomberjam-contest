using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace Bomberjam.Website.Storage
{
    public sealed class AzureStorageBotStorage : BaseBotStorage, IBotStorage
    {
        private const string BotsContainerName = "bots";
        private const string GamesContainerName = "games2";

        private readonly string _connectionString;

        public AzureStorageBotStorage(string connectionString)
        {
            this._connectionString = connectionString;
            this.EnsureContainerCreated(BotsContainerName);
            this.EnsureContainerCreated(GamesContainerName);
        }

        private void EnsureContainerCreated(string containerName)
        {
            var blobService = new BlobServiceClient(this._connectionString);
            var containerClient = blobService.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();
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
            var blobService = new BlobServiceClient(this._connectionString);
            var containerClient = blobService.GetBlobContainerClient(BotsContainerName);
            var blobClient = containerClient.GetBlobClient(MakeBotFileName(botId, isCompiled));

            await using (fileStream)
            {
                await blobClient.UploadAsync(fileStream, true).ConfigureAwait(false);
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

        private async Task<Stream> DownloadBot(Guid botId, bool isCompiled)
        {
            var blobService = new BlobServiceClient(this._connectionString);
            var containerClient = blobService.GetBlobContainerClient(BotsContainerName);
            var blobClient = containerClient.GetBlobClient(MakeBotFileName(botId, isCompiled));

            var botStream = new MemoryStream();
            using var response = await blobClient.DownloadToAsync(botStream).ConfigureAwait(false);
            botStream.Seek(0, SeekOrigin.Begin);
            return botStream;
        }

        public async Task UploadGameResult(Guid botId, Stream fileStream)
        {
            var blobService = new BlobServiceClient(this._connectionString);
            var containerClient = blobService.GetBlobContainerClient(GamesContainerName);
            var blobClient = containerClient.GetBlobClient(MakeGameFileName(botId));

            await using (fileStream)
            {
                await blobClient.UploadAsync(fileStream, true).ConfigureAwait(false);
            }
        }

        public async Task<Stream> DownloadGameResult(Guid botId)
        {
            var blobService = new BlobServiceClient(this._connectionString);
            var containerClient = blobService.GetBlobContainerClient(GamesContainerName);
            var blobClient = containerClient.GetBlobClient(MakeGameFileName(botId));

            var gameStream = new MemoryStream();
            using var response = await blobClient.DownloadToAsync(gameStream).ConfigureAwait(false);
            gameStream.Seek(0, SeekOrigin.Begin);
            return gameStream;
        }
    }
}