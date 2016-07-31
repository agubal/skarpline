using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Skarpline.Api.OAuth;
using Skarpline.Services.Users;

namespace Skarpline.Api
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        /// <summary>
        /// Apply configuration for authorization logic
        /// </summary>
        /// <param name="app">App config object</param>
        public void ConfigureOAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(_container.GetInstance<IUserService>()),
                AccessTokenFormat = new TicketDataFormat(app.CreateDataProtector(
               typeof(OAuthAuthorizationServerMiddleware).Namespace, "Access_Token", "v1")),
                RefreshTokenFormat = new TicketDataFormat(app.CreateDataProtector(
                 typeof(OAuthAuthorizationServerMiddleware).Namespace, "Refresh_Token", "v1"))
            };

            OAuthBearerOptions.AccessTokenFormat = oAuthServerOptions.AccessTokenFormat;
            OAuthBearerOptions.AccessTokenProvider = oAuthServerOptions.AccessTokenProvider;
            OAuthBearerOptions.AuthenticationMode = oAuthServerOptions.AuthenticationMode;
            OAuthBearerOptions.AuthenticationType = oAuthServerOptions.AuthenticationType;
            OAuthBearerOptions.Description = oAuthServerOptions.Description;

            OAuthBearerOptions.SystemClock = oAuthServerOptions.SystemClock;
            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}