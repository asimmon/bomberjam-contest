﻿using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Bomberjam.Website.Github;

namespace Bomberjam.Website.Storage
{
    public sealed class AzureStorageBomberjamStorage : BaseBomberjamStorage
    {
        private const string BotsContainerName = "bots";
        private const string GamesContainerName = "games";
        private const string StartersContainerName = "starters";

        private readonly string _connectionString;

        public AzureStorageBomberjamStorage(string connectionString)
        {
            this._connectionString = connectionString;
            this.EnsureContainerCreated(BotsContainerName);
            this.EnsureContainerCreated(GamesContainerName);
            this.EnsureContainerCreated(StartersContainerName);
        }

        private void EnsureContainerCreated(string containerName)
        {
            this.CreateClient().GetBlobContainerClient(containerName).CreateIfNotExists();
        }

        protected override async Task UploadBot(Guid botId, Stream fileStream, bool isCompiled)
        {
            var containerClient = this.CreateClient().GetBlobContainerClient(BotsContainerName);
            var blobClient = containerClient.GetBlobClient(MakeBotFileName(botId, isCompiled));

            await using (fileStream)
            {
                await blobClient.UploadAsync(fileStream, true).ConfigureAwait(false);
            }
        }

        protected override async Task DownloadBotTo(Guid botId, Stream destinationStream, bool isCompiled)
        {
            var containerClient = this.CreateClient().GetBlobContainerClient(BotsContainerName);
            var blobClient = containerClient.GetBlobClient(MakeBotFileName(botId, isCompiled));
            await blobClient.DownloadToAsync(destinationStream).ConfigureAwait(false);
        }

        public override async Task UploadGameResult(Guid botId, Stream fileStream)
        {
            var containerClient = this.CreateClient().GetBlobContainerClient(GamesContainerName);
            var blobClient = containerClient.GetBlobClient(MakeGameFileName(botId));

            await using (fileStream)
            {
                await blobClient.UploadAsync(fileStream, true).ConfigureAwait(false);
            }
        }

        public override async Task DownloadGameResultTo(Guid botId, Stream destinationStream)
        {
            var containerClient = this.CreateClient().GetBlobContainerClient(GamesContainerName);
            var blobClient = containerClient.GetBlobClient(MakeGameFileName(botId));
            await blobClient.DownloadToAsync(destinationStream).ConfigureAwait(false);
        }

        public override async Task UploadStarter(StarterOs os, Stream fileStream)
        {
            var containerClient = this.CreateClient().GetBlobContainerClient(StartersContainerName);
            var blobClient = containerClient.GetBlobClient(MakeStarterFileName(os));

            await using (fileStream)
            {
                await blobClient.UploadAsync(fileStream, true).ConfigureAwait(false);
            }
        }

        public override async Task DownloadStarter(StarterOs os, Stream destinationStream)
        {
            var containerClient = this.CreateClient().GetBlobContainerClient(StartersContainerName);
            var blobClient = containerClient.GetBlobClient(MakeStarterFileName(os));
            await blobClient.DownloadToAsync(destinationStream).ConfigureAwait(false);
        }

        private BlobServiceClient CreateClient() => new BlobServiceClient(this._connectionString);
    }
}