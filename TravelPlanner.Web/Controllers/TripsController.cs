using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Http;
using TravelPlanner.DB;
using TravelPlanner.Web.Common;
using TravelPlanner.Web.Extensions;

namespace TravelPlanner.Web.Controllers
{
    [Authorize(Roles = "User, Admin")]
    public class TripsController : ApiController
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly TravelPlannerDbProvider dbProvider;
        private readonly ObjectMapper mapper;

        public TripsController(TravelPlannerDbProvider dbProvider, ObjectMapper mapper)
        {
            this.dbProvider = dbProvider;
            this.mapper = mapper;
        }

        // GET: api/users/{userId}/trips
        public IEnumerable<Models.Trip> Get(long userId, 
            [FromUri] string destination = null, 
            [FromUri] DateTime? fromStart = null,
            [FromUri] DateTime? fromEnd = null,
            [FromUri] DateTime? toStart = null,
            [FromUri] DateTime? toEnd = null,
            [FromUri] string comment = null)
        {
            ApiControllerExtensions.EnsureCurrentUser(this, userId);

            using (var db = dbProvider.Get())
            {
                var qr = db.Trips
                           .Where(t => t.UserId == userId);

                qr = AddFilters(qr, destination, comment, fromStart, fromEnd, toStart, toEnd);

                return qr.ToList()
                         .Select(t => mapper.Map<Models.Trip>(t))
                         .ToList();
            }
        }

        private IQueryable<Trip> AddFilters(IQueryable<Trip> qr, 
            string destination, string comment,
            DateTime? fromStart, DateTime? fromEnd, 
            DateTime? toStart, DateTime? toEnd)
        {
            if (!string.IsNullOrWhiteSpace(destination))
            {
                qr = qr.Where(t => t.Destination == destination);
            }

            if (fromStart.HasValue)
            {
                fromStart = fromStart.Value.Date;
                qr = qr.Where(t => t.StartDate >= fromStart.Value);
            }

            if (fromEnd.HasValue)
            {
                fromEnd = fromEnd.Value.Date;
                qr = qr.Where(t => t.StartDate <= fromEnd.Value);
            }

            if (toStart.HasValue)
            {
                toStart = toStart.Value.Date;
                qr = qr.Where(t => t.EndDate >= toStart.Value);
            }

            if (toEnd.HasValue)
            {
                toEnd = toEnd.Value.Date;
                qr = qr.Where(t => t.EndDate <= toEnd.Value);
            }

            if (!string.IsNullOrWhiteSpace(comment))
            {
                qr = qr.Where(t => t.Comment.Contains(comment));
            }

            return qr;
        }

        // GET: api/users/{userId}/trips/{id}
        public IHttpActionResult Get(long userId, long id)
        {
            ApiControllerExtensions.EnsureCurrentUser(this, userId);

            using (var db = dbProvider.Get())
            {
                var dbTrip = db.Trips
                               .FirstOrDefault(t => t.Id == id && t.UserId == userId);

                if (dbTrip == null)
                {
                    return NotFound();
                }

                var trip = mapper.Map<Models.Trip>(dbTrip);
                return Ok(trip);
            }
        }

        // POST: api/users/{userId}/trips
        public IHttpActionResult Post(long userId, Models.Trip trip)
        {
            ApiControllerExtensions.EnsureCurrentUser(this, userId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (trip == null)
            {
                return BadRequest();
            }

            using (var db = dbProvider.Get())
            {
                var dbTrip = mapper.Map<Trip>(trip);
                dbTrip.UserId = userId;

                var user = db.Users
                             .FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound();
                }

                var saved = db.Trips.Add(dbTrip);
                db.SaveChanges();

                logger.Debug("New trip id = " + saved.Id + " added for user " + userId);
            }

            return Ok();
        }

        // PUT: api/users/{userId}/trips/{id}
        public IHttpActionResult Put(long userId, long id, Models.Trip trip)
        {
            ApiControllerExtensions.EnsureCurrentUser(this, userId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var db = dbProvider.Get())
            {
                var dbUser = db.Users
                               .FirstOrDefault(u => u.Id == userId);

                if (dbUser == null)
                {
                    return NotFound();
                }

                if (dbUser.Id != userId)
                {
                    return BadRequest();
                }

                var dbTrip = mapper.Map<DB.Trip>(trip);
                dbTrip.Id = id;
                dbTrip.UserId = userId;

                db.Trips.AddOrUpdate(dbTrip);
                db.SaveChanges();

                logger.Debug("Trip id = " + id + " for user " + userId + " updated");
            }

            return Ok();
        }

        // DELETE: api/users/{userId}/trips/{id}
        public IHttpActionResult Delete(long userId, long id)
        {
            ApiControllerExtensions.EnsureCurrentUser(this, userId);

            using (var db = dbProvider.Get())
            {
                var trip = db.Trips
                             .FirstOrDefault(t => t.Id == id && t.UserId == userId);

                if (trip == null)
                {
                    return NotFound();
                }

                db.Trips.Remove(trip);
                db.SaveChanges();

                logger.Debug("Trip id = " + id + " for user " + userId + " deleted");

                return Ok();
            }
        }
    }
}
