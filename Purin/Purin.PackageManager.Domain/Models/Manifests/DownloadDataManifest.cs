namespace Purin.PackageManager.Domain.Models.Manifests
{
    public class DownloadDataManifest
    {
        public string Uri { get; set; } = "";
        public bool Remote { get; set; }
        public string TargetPath { get; set; } = "";
    }
}
