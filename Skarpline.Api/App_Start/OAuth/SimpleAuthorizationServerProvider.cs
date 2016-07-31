using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Skarpline.Common.Results;
using Skarpline.Entities.Domain.Identity;
using Skarpline.Services.Users;

namespace Skarpline.Api.OAuth
{
    /// <summary>
    /// Authorization provider
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private int MaximumUsersInChat
        {
            get
            {
                string value = ConfigurationManager.AppSettings["MaximumUsersInChat"];
                return int.Parse(value);
            }
        }

        private readonly IUserService _userService;

        public SimpleAuthorizationServerProvider(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// To handle validation logic
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// To apply edition validation, check user and prepare authorization token
        /// </summary>
        /// <param name="context">Authorization context</param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

            if (context.OwinContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.OwinContext.Response.Headers.SetValues("Access-Control-Allow-Origin", allowedOrigin);
            }
            else
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            }
           
            ServiceResult<User> serviceResult = await _userService.FindUserAsync(context.UserName);
            if (serviceResult == null)
            {
                context.SetError("invalid_grant", "User not found");
                return;
            }

            //Check if we have space in chat:
            if (UserHandler.ConnectedIds.Count >= MaximumUsersInChat)
            {
                context.SetError("invalid_grant", $"Too many users in chat. Maximum {MaximumUsersInChat} users is allowed");
                return;
            }

            if (!serviceResult.Succeeded)
            {
                context.SetError("invalid_grant", string.Join(", ", serviceResult.ErrorMessage));
                return;
            }

            User user = serviceResult.Result;
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ConfigurationManager.AppSettings["Claim.Sub"], context.UserName));
            identity.AddClaim(new Claim(ConfigurationManager.AppSettings["Claim.Role"], "user"));
            identity.AddClaim(new Claim(ConfigurationManager.AppSettings["Claim.UserName"], context.UserName));
            identity.AddClaim(new Claim(ConfigurationManager.AppSettings["Claim.UserId"], user.Id));


            var properties = new Dictionary<string, string>
            {
                {"as:client_id", context.ClientId ?? string.Empty},
                {"userName", context.UserName},
                {"userId", user.Id},
                {"role", "user"}
            };

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties(properties));
            context.Validated(ticket);
        }

        /// <summary>
        /// To includes additional data in Token
        /// </summary>
        /// <param name="context">Authorization context</param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
    }
}