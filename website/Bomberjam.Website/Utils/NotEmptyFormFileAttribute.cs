using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Bomberjam.Website.Utils
{
    public sealed class NotEmptyFormFileAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value is IFormFile { Length: 0 }
                ? new ValidationResult(this.ErrorMessage)
                : ValidationResult.Success;
        }
    }
}