using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Bomberjam.Website.Github
{
    public sealed class DownloadGithubArtifactsOnStartup : BackgroundService
    {
        private readonly IGithubArtifactManager _downloader;

        public DownloadGithubArtifactsOnStartup(IGithubArtifactManager downloader)
        {
            this._downloader = downloader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this._downloader.Initialize(stoppingToken);
        }
    }
}