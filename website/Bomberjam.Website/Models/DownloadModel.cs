using Bomberjam.Website.Github;

namespace Bomberjam.Website.Models
{
    public class DownloadModel
    {
        public DownloadModel(StarterOs Os, bool showWhatsNext)
        {
            this.Os = Os;
            this.ShowWhatsNext = showWhatsNext;
        }

        public StarterOs Os { get; }

        public bool ShowWhatsNext { get; }
    }
}