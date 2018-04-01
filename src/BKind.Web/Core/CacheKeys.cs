using System;

namespace BKind.Web.Core
{
    public static class CacheKeys
    {
        public static Func<string, string> UserWithUsername = username => $"username-{username.ToLower()}"; 
    }
}