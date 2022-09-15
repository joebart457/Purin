using Purin.PackageManager.Server.Services;
using Sql;

namespace Purin.PackageManager.Server.Contexts
{
    public static class DataContext
    {
        private static SqliteClient? _client;
        public static SqliteClient Connection { get; private set; }

        private static SqliteClient BuildConnection()
        {
            if (_client == null)
            {
                _client = new SqliteClient(ConfigService.AppSettings.ConnectionString);
            }
            return _client;
            
        }
    }
}
