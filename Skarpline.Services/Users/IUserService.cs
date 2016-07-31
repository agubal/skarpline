using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Skarpline.Common.Results;
using Skarpline.Entities.Domain.Identity;

namespace Skarpline.Services.Users
{
    /// <summary>
    /// Business logic for User Entity
    /// </summary>
    public interface IUserService : IService<User>
    {
        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns></returns>
        Task<ServiceResult<IdentityResult>> RegisterUserAsync(User user);

        /// <summary>
        /// Finds user by Username and Password
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns></returns>
        Task<ServiceResult<User>> FindUserAsync(string userName);
    }
}
