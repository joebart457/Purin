using AppSettingsManager;
using Purin.PackageManager.Server.Models;

namespace Purin.PackageManager.Server.Services
{
    public static class ConfigService
    {
        public static ApplicationConfiguration AppSettings
        {
            get { return AppSettingsManager.AppSettings.Get<ApplicationConfiguration>(); }
            set { AppSettingsManager.AppSettings.Save(value); }
        }

        public static IpSafeList SafeList
        {
            get { return AppSettingsManager.AppSettings.Get<IpSafeList>("ipSafeList.json"); }
            set { AppSettingsManager.AppSettings.Save(value); }
        }


    }
}
