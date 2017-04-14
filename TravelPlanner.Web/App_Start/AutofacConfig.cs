using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using TravelPlanner.DB;
using TravelPlanner.Web.Common;

namespace TravelPlanner.Web
{
    public class AutofacConfig
    {
        public static IContainer Register(HttpConfiguration config, string connectionString)
        {
            var builder = new ContainerBuilder();
            var executingAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterControllers(executingAssembly);
            builder.RegisterApiControllers(executingAssembly);
            builder.RegisterWebApiFilterProvider(config);

            RegisterTypes(builder, connectionString);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            return container;
        }

        private static void RegisterTypes(ContainerBuilder builder, string connectionString)
        {
            var dbProvider = new TravelPlannerDbProvider(connectionString);

            builder.RegisterInstance(dbProvider);
            builder.RegisterInstance(new ObjectMapper());
            builder.RegisterInstance(new PasswordService());
            builder.RegisterInstance<IExceptionLogger>(new ExceptionLogger());
        }
    }
}
