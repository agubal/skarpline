using System.Collections.Generic;

namespace Skarpline.Api.OAuth
{
    /// <summary>
    /// Simple storage for chat sessions
    /// </summary>
    public static class UserHandler
    {
        public static List<string> ConnectedIds = new List<string>();
    }
}