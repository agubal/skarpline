using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Skarpline.Entities.Domain.Identity;

namespace Skarpline.Services.Identity
{
    /// <summary>
    /// Inherits from UserManager to ovveride some default behavior
    /// </summary>
    public class ChatUserManager : UserManager<User>
    {
        public ChatUserManager(IUserStore<User> store)
            : base(store)
        {
            var options = new IdentityFactoryOptions<ChatUserManager>();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    TokenLifespan = TimeSpan.FromDays(7)
                };
            }

            //Configure validation logic for usernames
            UserValidator = new ChatUserValidator<User>(this);

            //Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
        }
    }
}
