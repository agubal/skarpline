using Skarpline.Common.Identity;

namespace Skarpline.Entities.Models.Identity
{
    /// <summary>
    /// View model for User
    /// </summary>
    public class UserModel : IIdentifier<string>
    {
        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public string Id { get; set; }

    }
}
