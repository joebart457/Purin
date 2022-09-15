using AppSettingsManager;
using Purin.PackageManager.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.PackageManager.Client.Services
{
    internal static class ConfigService
    {
        public static Configuration Settings
        {
            get { return AppSettings.Get<Configuration>(); }
            set { AppSettings.Save(value); }
        }
    }
}
