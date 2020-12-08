using System.ComponentModel.DataAnnotations;

namespace Bomberjam.Website.Models
{
    public class AccountEditViewModel
    {
        [Required]
        [StringLength(32)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}
