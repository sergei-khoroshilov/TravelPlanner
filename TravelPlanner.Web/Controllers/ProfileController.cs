using Microsoft.AspNet.Identity;
using NLog;
using System.Linq;
using System.Web.Http;
using TravelPlanner.DB;
using TravelPlanner.Web.Common;

namespace TravelPlanner.Web.Controllers
{
    [Authorize]
    public class ProfileController : ApiController
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly TravelPlannerDbProvider dbProvider;
        private readonly ObjectMapper mapper;

        public ProfileController(TravelPlannerDbProvider dbProvider, ObjectMapper mapper)
        {
            this.dbProvider = dbProvider;
            this.mapper = mapper;
        }

        // GET: /api/profile
        public IHttpActionResult Get()
        {
            long userId = User.Identity.GetUserId<long>();

            logger.Debug("/api/profile request received for user " + userId);

            using (var db = dbProvider.Get())
            {
                var dbUser = db.Users
                               .FirstOrDefault(u => u.Id == userId);

                if (dbUser == null)
                {
                    logger.Error("/api/profile request received for not authorized user");
                    return BadRequest();
                }

                var user = mapper.Map<Models.User>(dbUser);
                return Ok(user);
            }
        }
    }
}
