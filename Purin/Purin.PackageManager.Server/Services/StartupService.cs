using Logger;

namespace Purin.PackageManager.Server.Services
{
    public static class StartupService
    {

        public static void Configure()
        {
            CliLogger.LoggingEnabled = ConfigService.AppSettings.LoggingEnabled;
        }
    }
}
