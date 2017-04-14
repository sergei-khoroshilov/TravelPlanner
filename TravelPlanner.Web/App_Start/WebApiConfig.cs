using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FluentValidation.WebApi;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using TravelPlanner.DB;
using TravelPlanner.Web.Common;

namespace TravelPlanner.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Services.Add(typeof(IExceptionLogger), new ExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "UserTripsApi",
                routeTemplate: "api/users/{userId}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            RegisterValidation(config);
        }

        private static void RegisterValidation(HttpConfiguration config)
        {
            FluentValidationModelValidatorProvider.Configure(config);
        }
    }
}
