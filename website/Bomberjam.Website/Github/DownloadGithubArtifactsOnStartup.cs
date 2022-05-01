using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Github
{
    public sealed class DownloadGithubArtifactsOnStartup : BackgroundService
    {
        private readonly IGithubArtifactManager _downloader;
        private readonly ILogger _logger;

        public DownloadGithubArtifactsOnStartup(IGithubArtifactManager downloader, ILogger<DownloadGithubArtifactsOnStartup> logger)
        {
            this._downloader = downloader;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await this._downloader.Initialize(stoppingToken);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An error occurred while downloading latest github artifacts");
            }
        }
    }
}