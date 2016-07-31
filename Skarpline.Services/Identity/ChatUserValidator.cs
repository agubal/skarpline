using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Skarpline.Entities.Domain.Identity;

namespace Skarpline.Services.Identity
{
    /// <summary>
    /// Custom user validator
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class ChatUserValidator<TUser> : IIdentityValidator<TUser> where TUser : class, IUser
    {
        private readonly UserManager<TUser> _manager;

        public ChatUserValidator() { }

        public ChatUserValidator(UserManager<TUser> manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Applyes User validation
        /// </summary>
        /// <param name="item">User</param>
        /// <returns>Validation result</returns>
		public async Task<IdentityResult> ValidateAsync(TUser item)
        {
            var emailRegex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(item.UserName))
            {
                errors.Add("Email address is missing.");
            }
            else if (!emailRegex.IsMatch(item.UserName))
            {
                errors.Add("Email format is invalid or email contains invalid chars.");
            }

            if (_manager == null)
            {
                return errors.Any()
                    ? IdentityResult.Failed(errors.ToArray())
                    : IdentityResult.Success;
            }

            var otherAccount = await _manager.FindByNameAsync(item.UserName);
            if (otherAccount != null && otherAccount.Id != item.Id)
            {
                var user = otherAccount as User;
                if (user != null && !user.EmailConfirmed)
                {
                    await _manager.DeleteAsync(otherAccount);
                }
                else
                {
                    errors.Add("Select a different email address. An account has already been created with this email address.");
                }
            }

            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }
    }
}
