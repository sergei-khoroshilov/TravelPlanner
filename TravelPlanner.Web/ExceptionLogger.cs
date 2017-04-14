using NLog;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace TravelPlanner.Web
{
    public class ExceptionLogger : IExceptionLogger
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            logger.Error(context.Exception);
            return Task.FromResult(0);
        }
    }
}