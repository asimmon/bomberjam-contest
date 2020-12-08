using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Bomberjam.Website.Utils
{
    public sealed class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly HashSet<string> _extensions;

        public AllowedExtensionsAttribute(params string[] extensions)
        {
            if (extensions.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(extensions));

            this._extensions = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!this._extensions.Contains(extension))
                {
                    return new ValidationResult(this.ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}