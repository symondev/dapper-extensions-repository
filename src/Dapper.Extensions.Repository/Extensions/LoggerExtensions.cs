using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dapper.Extensions.Repository.Extensions
{
    internal static class LoggerExtensions
    {
        public static void LogDbContext(this ILogger logger, string message)
        {
            logger?.LogDebug(message);
        }

        public static void LogSql(this ILogger logger, string sql, object param = null)
        {
            if (logger != null)
            {
                var message = $"Execute sql: {sql}";
                if (param != null)
                {
                    message += $", {JsonConvert.SerializeObject(param)}.";
                }

                logger.LogInformation(message);
            }
        }
    }
}