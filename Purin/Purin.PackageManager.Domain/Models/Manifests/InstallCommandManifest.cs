namespace Purin.PackageManager.Domain.Models.Manifests
{
    public class InstallCommandManifest
    {
        public string Cmd { get; set; } = "";
        public bool IgnoreOnFail { get; set; }
        public List<int> ExitCodeSuccess { get; set; } = new List<int>();
        public int Stage { get; set; }
    }
}
