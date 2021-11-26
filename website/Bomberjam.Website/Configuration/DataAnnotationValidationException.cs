using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bomberjam.Website.Configuration
{
    public class DataAnnotationValidationException : Exception
    {
        public DataAnnotationValidationException(ValidationContext validationContext, IReadOnlyList<ValidationResult> results)
            : base(FormatErrorMessage(validationContext, results))
        {
            this.Results = results.ToList();
        }

        public IReadOnlyList<ValidationResult> Results { get; }

        private static string FormatErrorMessage(ValidationContext validationContext, IReadOnlyList<ValidationResult> results)
        {
            var errorMessages = results.Select(r => r.ErrorMessage);
            return $"Validation for '{validationContext.DisplayName}' failed: {string.Join(", ", errorMessages)}";
        }
    }
}