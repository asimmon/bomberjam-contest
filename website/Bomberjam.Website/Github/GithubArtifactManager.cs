using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Bomberjam.Website.Configuration;
using Bomberjam.Website.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bomberjam.Website.Github
{
    public sealed class GithubArtifactManager : IGithubArtifactManager
    {
        private const string WindowsArtifactName = "bomberjam-starter-kits-win-x64";
        private const string LinuxArtifactName = "bomberjam-starter-kits-linux-x64";
        private const string MacOsArtifactName = "bomberjam-starter-kits-osx-x64";

        private static readonly IReadOnlyDictionary<StarterOs, string> ArtifactNameByOs = new Dictionary<StarterOs, string>
        {
            [StarterOs.Windows] = WindowsArtifactName,
            [StarterOs.Linux] = LinuxArtifactName,
            [StarterOs.MacOs] = MacOsArtifactName,
        };

        private readonly HttpClient _httpClient;
        private readonly GitHubOptions _githubOptions;
        private readonly IBomberjamStorage _storage;
        private readonly ILogger<GithubArtifactManager> _logger;

        public GithubArtifactManager(IOptions<GitHubOptions> githubOptions, IHttpClientFactory httpClientFactory, IBomberjamStorage storage, ILogger<GithubArtifactManager> logger)
        {
            this._logger = logger;
            this._httpClient = httpClientFactory.CreateClient(nameof(GithubArtifactManager));
            this._storage = storage;
            this._githubOptions = githubOptions.Value;
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            this._logger.LogInformation("Finding last workflow run");
            var run = await this.FindLastSuccessfulWorkflowRun(cancellationToken);
            this._logger.LogInformation("Last workflow run is {LastWorkflowRun}", run.Id);

            this._logger.LogInformation("Finding workflow run {LastWorkflowRun} artifacts", run.Id);
            var artifacts = await this.GetWorkflowRunArtifacts(run.Id, cancellationToken);
            this._logger.LogInformation("Workflow run {LastWorkflowRun} artifacts have been found", run.Id);

            this._logger.LogInformation("Downloading workflow run {LastWorkflowRun} artifacts", run.Id);
            var tasks = new List<Task>(ArtifactNameByOs.Count);

            foreach (var (os, artifactName) in ArtifactNameByOs)
            {
                var artifact = artifacts.FirstOrDefault(x => artifactName.Equals(x.Name, StringComparison.Ordinal));
                if (artifact == null)
                    throw new Exception($"Could not find a workflow run {run.Id} artifact with name {os} for OS {artifactName}");

                if (artifact.IsExpired)
                    throw new Exception($"Artifact {artifact.Name} of workflow run {run.Id} is expired");

                var task = this.DownloadAndCacheArtifactBytes(artifact, os, cancellationToken);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            this._logger.LogInformation("Workflow run {LastWorkflowRun} artifacts have been downloaded and cached in memory", run.Id);
        }

        private async Task DownloadAndCacheArtifactBytes(Artifact artifact, StarterOs os, CancellationToken cancellationToken)
        {
            await using var fromStream = await this._httpClient.GetStreamAsync(artifact.DownloadUrl, cancellationToken);
            await this._storage.UploadStarter(os, fromStream);
        }

        public async Task DownloadTo(StarterOs os, Stream stream)
        {
            await this._storage.DownloadStarter(os, stream);
        }

        private async Task<WorkflowRun> FindLastSuccessfulWorkflowRun(CancellationToken cancellationToken)
        {
            const string requestUriFormat = "https://api.github.com/repos/asimmon/bomberjam-contest/actions/runs?branch=master&status=success&exclude_pull_requests=true&page={0}";

            for (var pageNumber = 1;; pageNumber++)
            {
                var page = await this._httpClient.GetFromJsonAsync<WorkflowRunPage>(string.Format(requestUriFormat, pageNumber), cancellationToken);

                if (page == null)
                    throw new InvalidOperationException("Github returned null when retrieving workflow runs");

                if (page.Runs.Count == 0)
                    throw new InvalidOperationException("Could not find a successful run after fetching all the pages");

                var run = page.Runs.FirstOrDefault(x => x.WorkflowId == this._githubOptions.ArtifactsWorkflowId);
                if (run != null)
                    return run;
            }
        }

        private async Task<IReadOnlyList<Artifact>> GetWorkflowRunArtifacts(int runId, CancellationToken cancellationToken)
        {
            const string requestUriFormat = "https://api.github.com/repos/asimmon/bomberjam-contest/actions/runs/{0}/artifacts";
            var page = await this._httpClient.GetFromJsonAsync<ArtifactPage>(string.Format(requestUriFormat, runId), cancellationToken);

            if (page == null)
                throw new InvalidOperationException($"Github returned null when retrieving workflow run {runId} artifacts");

            if (page.Count == 0)
                throw new InvalidOperationException($"Workflow run {runId} has no artifacts");

            return page.Artifacts;
        }

        private sealed class WorkflowRunPage
        {
            [JsonPropertyName("workflow_runs")]
            public List<WorkflowRun> Runs { get; set; } = new List<WorkflowRun>();
        }

        private sealed class WorkflowRun
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("workflow_id")]
            public int WorkflowId { get; set; }
        }

        private sealed class ArtifactPage
        {
            [JsonPropertyName("total_count")]
            public int Count { get; set; }

            [JsonPropertyName("artifacts")]
            public List<Artifact> Artifacts { get; set; } = new List<Artifact>();
        }

        private sealed class Artifact
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("archive_download_url")]
            public string DownloadUrl { get; set; } = string.Empty;

            [JsonPropertyName("expired")]
            public bool IsExpired { get; set; }
        }
    }
}