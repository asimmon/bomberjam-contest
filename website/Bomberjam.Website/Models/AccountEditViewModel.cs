using System.ComponentModel.DataAnnotations;

namespace Bomberjam.Website.Models
{
    public class AccountEditViewModel
    {
        private string _userName;

        public AccountEditViewModel()
        {
        }

        public AccountEditViewModel(User user)
        {
            this.UserName = user.UserName;
        }

        [Required]
        [MinLength(2)]
        [StringLength(32)]
        [Display(Name = "Username")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Username can only contain alphanumeric characters (a-z, A-Z, 0-9)")]
        public string UserName
        {
            get => this._userName;
            set => this._userName = value?.Trim();
        }
    }
}
