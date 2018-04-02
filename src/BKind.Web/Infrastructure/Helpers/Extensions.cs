using System;
using System.Text.RegularExpressions;

namespace BKind.Web.Infrastructure.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Uwrap most inner exception
        /// </summary>
        public static Exception Unwrap(this Exception exception)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            return exception;
        }

        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }
    }
}