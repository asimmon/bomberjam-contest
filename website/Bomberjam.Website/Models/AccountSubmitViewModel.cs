using System.ComponentModel.DataAnnotations;
using Bomberjam.Website.Utils;
using Microsoft.AspNetCore.Http;

namespace Bomberjam.Website.Models
{
    public class AccountSubmitViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Select a .zip file to upload your bot")]
        [Required(ErrorMessage = "You must select a .zip file")]
        [AllowedExtensions(".zip", ErrorMessage = "You must select a .zip file")]
        [NotEmptyFormFileAttribute(ErrorMessage = "The .zip file cannot be empty")]
        public IFormFile BotFile { set; get; }
    }
}