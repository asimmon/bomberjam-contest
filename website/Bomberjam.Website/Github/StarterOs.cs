using System.ComponentModel.DataAnnotations;

namespace Bomberjam.Website.Github
{
    public enum StarterOs
    {
        [Display(Name = "Windows")]
        Windows,

        [Display(Name = "Linux")]
        Linux,

        [Display(Name = "macOS")]
        MacOs
    }
}