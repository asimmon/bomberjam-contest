using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Bomberjam.Website.Github
{
    public interface IGithubArtifactManager
    {
        public Task Initialize(CancellationToken cancellationToken);
        public Task DownloadTo(StarterOs os, Stream stream);
    }
}