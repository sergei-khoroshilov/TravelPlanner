using Microsoft.Owin.Security.OAuth;
using NLog;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelPlanner.DB;

namespace TravelPlanner.Web
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly TravelPlannerDbProvider dbProvider;
        private readonly PasswordService passwordService;

        public AuthorizationServerProvider(TravelPlannerDbProvider dbProvider, PasswordService passwordService)
        {
            this.dbProvider = dbProvider;
            this.passwordService = passwordService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            logger.Debug("/api/token request received for user " + context.UserName);

            using (var db = dbProvider.Get())
            {
                var user = db.Users
                             .FirstOrDefault(u => u.Email == context.UserName);

                if (user == null || !passwordService.IsValid(context.Password, user.Password))
                {
                    logger.Debug("Invalid credentials for user " + context.UserName);
                    context.SetError("Invalid credentials");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaims(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Type.ToString())
                });

                context.Validated(identity);

                logger.Info("token sent to user " + context.UserName);
                return;
            }
        }
    }
}