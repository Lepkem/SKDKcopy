using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Data.Helpers
{
    public static class StandardMessages
    {
        public static string RequiredField(string fieldName)
        {
            fieldName = fieldName.First().ToString().ToUpper() + fieldName.Substring(1);
            return $"Oops! {fieldName} is een verplicht veld.";
        }

        public static string AmountOfCharacters()
        {
            return $"De hoeveelheid karakters is onjuist.";
        }

        public static string CapitalizeFirst(string input)
        {
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
