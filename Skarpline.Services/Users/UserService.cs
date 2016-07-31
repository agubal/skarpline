using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Skarpline.Common.Results;
using Skarpline.Data.Core;
using Skarpline.Entities.Domain.Identity;
using Skarpline.Services.Identity;

namespace Skarpline.Services.Users
{
    /// <summary>
    /// Business logic for User Entity
    /// </summary>
    public class UserService : GenericService<User>, IUserService
    {
        private readonly ChatUserManager _userManager;

        public UserService(IRepository<User> entityRepo, IUnitOfWork work, ChatUserManager userManager) : base(entityRepo, work)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns></returns>
        public async Task<ServiceResult<IdentityResult>> RegisterUserAsync(User user)
        {
            if (user == null) return new ServiceResult<IdentityResult>("User was not provided");

            //Create user
            IdentityResult userCreateResult = await _userManager.CreateAsync(user);
            return !userCreateResult.Succeeded
                ? new ServiceResult<IdentityResult>(userCreateResult.Errors)
                : new ServiceResult<IdentityResult>(userCreateResult);
        }

        /// <summary>
        /// Finds user by Username and Password
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns></returns>
        public async Task<ServiceResult<User>> FindUserAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return new ServiceResult<User>("Username was not provided");
            var user = await _userManager.Users.AsNoTracking().SingleOrDefaultAsync(x => x.UserName == userName);
            return new ServiceResult<User> { Result = user };
        }
    }
}
