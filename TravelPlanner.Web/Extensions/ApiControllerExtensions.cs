using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TravelPlanner.Web.Extensions
{
    public static class ApiControllerExtensions
    {
        public static void EnsureCurrentUser(this ApiController controller, long requestUserId)
        {
            if (controller.User.IsInRole("User"))
            {
                long currentUserId = controller.User.Identity.GetUserId<long>();

                if (requestUserId != currentUserId)
                {
                    var forbiddenResponse = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    throw new HttpResponseException(forbiddenResponse);
                }
            }
        }
    }
}
