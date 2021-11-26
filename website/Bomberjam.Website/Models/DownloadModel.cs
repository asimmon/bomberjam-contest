using Bomberjam.Website.Github;

namespace Bomberjam.Website.Models
{
    public class DownloadModel
    {
        public DownloadModel(StarterOs Os)
        {
            this.Os = Os;
        }

        public StarterOs Os { get; }
    }
}