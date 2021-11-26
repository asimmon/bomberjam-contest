using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Bomberjam.Website.Configuration
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class NotEmptyAttribute : ValidationAttribute
    {
        private const string ErrorMessageFormat = "The {0} field must be an enumerable that contains at least one element";

        public NotEmptyAttribute()
            : base(ErrorMessageFormat)
        {
        }

        public override bool IsValid(object value) => value switch
        {
            null => true,
            ICollection<object> genericCollection => genericCollection.Count > 0,
            ICollection nonGenericCollection => nonGenericCollection.Count > 0,
            IEnumerable<object> genericEnumerable => genericEnumerable.Any(),
            IEnumerable nonGenericCollection => nonGenericCollection.Cast<object>().Any(),
            _ => false,
        };
    }
}