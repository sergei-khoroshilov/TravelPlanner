using System.Web.Mvc;

namespace TravelPlanner.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
