using Autofac;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelPlanner.DB;

namespace TravelPlanner.Web
{
    public class AuthConfig
    {
        public static void Register(IAppBuilder app, IContainer container)
        {
            var dbProvider = container.Resolve<TravelPlannerDbProvider>();
            var passwordService = container.Resolve<PasswordService>();

            var oauthOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider(dbProvider, passwordService),
                AllowInsecureHttp = true
            };

            app.UseOAuthAuthorizationServer(oauthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}