using CliParser;
using Logger;
using Purin.PackageManager.Client.Services;

public static class Program
{
    public static void Main(string[] args)
    {
        AppSettingsManager.AppSettings.GenerateIfNotFound = true;
        args.ResolveWithTryCatch(typeof(ProgramStartupService), 
            ex => 
            {
                CliLogger.LogError($"ERROR --- {ex.Message}");
            });
    }
}