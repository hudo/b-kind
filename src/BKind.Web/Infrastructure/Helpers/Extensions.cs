using System;

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
    }
}