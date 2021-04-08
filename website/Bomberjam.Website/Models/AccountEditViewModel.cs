using System.ComponentModel.DataAnnotations;

namespace Bomberjam.Website.Models
{
    public class AccountEditViewModel
    {
        private string _userName;
        private string _email;

        public AccountEditViewModel()
        {
        }

        public AccountEditViewModel(User user)
        {
            this.UserName = user.UserName;
            this.Email = user.Email;
        }

        [Required]
        [MinLength(2)]
        [StringLength(32)]
        [Display(Name = "Username")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Username can only contain alphanumericals (a-z, A-Z, 0-9)")]
        public string UserName
        {
            get => this._userName;
            set => this._userName = value?.Trim();
        }

        [Required]
        [StringLength(128)]
        [Display(Name = "Email address")]
        [RegularExpression(Constants.EmailRegexPattern, ErrorMessage = "Enter a valid email address")]
        public string Email
        {
            get => this._email;
            set => this._email = value?.Trim();
        }
    }
}
