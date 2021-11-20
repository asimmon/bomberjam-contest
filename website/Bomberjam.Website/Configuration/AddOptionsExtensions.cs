using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bomberjam.Website.Configuration
{
    public static class AddOptionsExtensions
    {
        public static TOptions AddOptionsAndValidate<TOptions>(this IServiceCollection services, IConfiguration configuration)
            where TOptions : class
        {
            // Enables lazy validation of the options when IOptions<> and other options interfaces are used later
            services.AddOptions<TOptions>().Bind(configuration).ValidateDataAnnotations();

            // Performs an immediate validation of the current options value
            var options = configuration.Get<TOptions>();
            return ValidateObjectDataAnnotations(options);
        }

        private static T ValidateObjectDataAnnotations<T>(T value)
            where T : class
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(value, serviceProvider: null, items: null);

            if (Validator.TryValidateObject(value, validationContext, validationResults, validateAllProperties: true))
            {
                return value;
            }

            throw new DataAnnotationValidationException(validationContext, validationResults);
        }
    }
}