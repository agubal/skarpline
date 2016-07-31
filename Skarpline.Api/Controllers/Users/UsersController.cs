using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Skarpline.Common.Results;
using Skarpline.Entities.Domain.Identity;
using Skarpline.Entities.Models.Identity;
using Skarpline.Services.Users;

namespace Skarpline.Api.Controllers.Users
{
    /// <summary>
    /// API Controller to work with User entity
    /// </summary>
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register new User
        /// </summary>
        /// <param name="userRegisterModel">Registration object</param>
        /// <returns></returns>
        [AllowAnonymous, Route("register")]
        public async Task<IHttpActionResult> PostRegister([FromBody]UserRegisterModel userRegisterModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                UserName = userRegisterModel.Email,
                Email = userRegisterModel.Email,
                EmailConfirmed = true
            };

            ServiceResult result = await _userService.RegisterUserAsync(user);
            var userModel = Mapper.Map<UserModel>(user);
            return GetErrorResult(result) ?? Created("DefaultApi", userModel);
        }
    }
}
