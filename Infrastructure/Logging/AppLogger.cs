using Microsoft.Extensions.Logging;

namespace Infrastructure.Logging
{
    public static class AppLogger
    {
        public static void LogInfo<T>(ILogger<T> logger, string message)
        {
            logger.LogInformation("[INFO] {Message} | Time: {Time}", message, DateTime.UtcNow);
        }

        public static void LogError<T>(ILogger<T> logger, Exception ex, string message)
        {
            logger.LogError(ex, "[ERROR] {Message} | Time: {Time}", message, DateTime.UtcNow);
        }

        public static void LogWarning<T>(ILogger<T> logger, string message)
        {
            logger.LogWarning("[WARN] {Message} | Time: {Time}", message, DateTime.UtcNow);
        }

        public static void LogDebug<T>(ILogger<T> logger, string message)
        {
            logger.LogDebug("[DEBUG] {Message} | Time: {Time}", message, DateTime.UtcNow);
        }
    }
}
