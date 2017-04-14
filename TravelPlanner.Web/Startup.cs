using Microsoft.Owin;
using Owin;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(TravelPlanner.Web.Startup))]

namespace TravelPlanner.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var config = GlobalConfiguration.Configuration;

            string connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

            var container = AutofacConfig.Register(config, connectionString);
            AuthConfig.Register(app, container);
        }
    }
}
