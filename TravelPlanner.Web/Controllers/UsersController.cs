using NLog;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;
using TravelPlanner.DB;
using TravelPlanner.Web.Common;
using TravelPlanner.Web.Extensions;

namespace TravelPlanner.Web.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly TravelPlannerDbProvider dbProvider;
        private readonly ObjectMapper mapper;
        private readonly PasswordService passwordService;

        public UsersController(TravelPlannerDbProvider dbProvider, ObjectMapper mapper, PasswordService passwordService)
        {
            this.dbProvider = dbProvider;
            this.mapper = mapper;
            this.passwordService = passwordService;
        }

        // GET: api/users
        [Authorize(Roles = "Admin, Manager")]
        public IEnumerable<Models.User> Get()
        {
            using (var db = dbProvider.Get())
            {
                return db.Users
                         .ToArray()
                         .Select(dbUser => mapper.Map<Models.User>(dbUser));
            }
        }

        // GET: api/users/{id}
        public IHttpActionResult Get(long id)
        {
            ApiControllerExtensions.EnsureCurrentUser(this, id);

            using (var db = dbProvider.Get())
            {
                var dbUser = db.Users
                               .FirstOrDefault(u => u.Id == id);

                if (dbUser == null)
                {
                    return NotFound(); 
                }

                var user = mapper.Map<Models.User>(dbUser);

                return Ok(user);
            }
        }

        // POST: api/users
        [AllowAnonymous]
        public IHttpActionResult Post(Models.UserWithPassword user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!User.Identity.IsAuthenticated || User.IsInRole("User"))
            {
                user.Type = UserType.User;
            }

            using(var db = dbProvider.Get())
            {
                var existingUser = db.Users
                                     .FirstOrDefault(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("user.Email", "Email is already used");
                    return BadRequest(ModelState);
                }
  
                var dbUser = mapper.Map<DB.User>(user);
                dbUser.Password = passwordService.GetHash(user.Password);

                var saved = db.Users.Add(dbUser);
                db.SaveChanges();

                logger.Debug("User " + user.Email + " added with id = " + saved.Id);
            }

            return Ok();
        }

        // PUT: api/users/{id}
        public IHttpActionResult Put(long id, Models.UserWithPassword user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiControllerExtensions.EnsureCurrentUser(this, id);

            if (User.IsInRole("User"))
            {
                user.Type = UserType.User;
            }

            using (var db = dbProvider.Get())
            {
                var dbUser = db.Users
                               .FirstOrDefault(u => u.Id == id);

                // Do not change email
                user.Email = dbUser.Email;

                if (dbUser == null)
                {
                    return NotFound();
                }

                dbUser = mapper.Map<DB.User>(user);
                dbUser.Password = passwordService.GetHash(user.Password);

                db.Users.AddOrUpdate(dbUser);
                db.SaveChanges();
            }

            logger.Debug("User id = " + id + " " + user.Email + " updated");

            return Ok();
        }
    }
}
