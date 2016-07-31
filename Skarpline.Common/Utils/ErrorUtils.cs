using System;

namespace Skarpline.Common.Utils
{
    /// <summary>
    /// Utils to handle Exceptions
    /// </summary>
    public class ErrorUtils
    {
        /// <summary>
        /// Method to get real exception message in recursive way
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="defaultException">Default exception if it failed to get message from Exception object</param>
        /// <returns></returns>
        public static string GetErrorMessage(Exception e, string defaultException)
        {
            if (e.InnerException != null) return GetErrorMessage(e.InnerException, defaultException);
            return !string.IsNullOrEmpty(e.Message) ? e.Message : defaultException;
        }
    }
}
