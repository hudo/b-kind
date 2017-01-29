using System;
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
    }
}