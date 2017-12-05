using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace BKind.Web.Infrastructure.Helpers
{
    public static class StringHelpers
    {
        public static string ComputeHash(string content, string salt)
        {
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: content,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public static string GenerateUniqueRandomToken()
        {
            const string availableChars = "0123456789abcdefghijklmnopqrstuvwxyz";
            using (var generator = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[8];
                generator.GetBytes(bytes);
                var chars = bytes.Select(b => availableChars[b % availableChars.Length]);
                var token = new string(chars.ToArray());
                return token;
            }
        }

        public static string Shorten(this string source, int length)
        {
            if (string.IsNullOrWhiteSpace(source)) return string.Empty;

            if (source.Length <= length) return source;

            var wordBreak = length;
            var wasCut = false;

            if (source[length] != ' ')
                wordBreak = source.IndexOf(' ', length);

            if (wordBreak <= 0)
                wordBreak = length;
            else
                wasCut = true;

            return source.Substring(0, wordBreak) + (wasCut ? "..." : "");
        }

        public static string ReadTime(this string story)
        {
            if (string.IsNullOrEmpty(story)) return "0";

            var wordCount = story.Split(' ').Length;

            return wordCount < 50 ? " < 1" : (wordCount / 250 + 1).ToString();
        }
    }
}