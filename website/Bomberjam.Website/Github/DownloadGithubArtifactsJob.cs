using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Github
{
    public sealed class DownloadGithubArtifactsJob
    {
        private readonly IGithubArtifactManager _downloader;
        private readonly ILogger _logger;

        public DownloadGithubArtifactsJob(IGithubArtifactManager downloader, ILogger<DownloadGithubArtifactsJob> logger)
        {
            this._downloader = downloader;
            this._logger = logger;
        }

        public async Task Run()
        {
            try
            {
                await this._downloader.Initialize(CancellationToken.None);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An error occurred while downloading latest github artifacts");
            }
        }
    }
}