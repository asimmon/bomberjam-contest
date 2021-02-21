using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Bomberjam.Website.Utils
{
    public static class EnumExtensions
    {
        private static readonly Dictionary<Enum, string> EnumValueDisplayStrings = new();

        public static string ToDisplayString(this Enum enumValue)
        {
            if (EnumValueDisplayStrings.TryGetValue(enumValue, out var displayString))
            {
                return displayString;
            }

            if (enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault() is { } member)
            {
                if (member.GetCustomAttribute<DisplayAttribute>(false) is { } attribute)
                {
                    return EnumValueDisplayStrings[enumValue] = attribute.Name;
                }
            }

            return EnumValueDisplayStrings[enumValue] = enumValue.ToString();
        }
    }
}