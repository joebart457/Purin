namespace Purin.PackageManager.Domain.Models.Manifests
{
    public class PackageManifest
    {
        public string Name { get; set; } = "";
        public string Tag { get; set; } = "";
        public string Description { get; set; } = "No description.";
        public DownloadDataManifest? DownloadDataManifest { get; set; }
        public List<ExtractArchiveManifest>? ArchivesToExtract { get; set; }
        public List<InstallCommandManifest>? InstallCommands { get; set; }
    }
}
