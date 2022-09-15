namespace Purin.PackageManager.Domain.Models.Manifests
{
    public class ExtractArchiveManifest
    {
        public string SourcePath { get; set; } = "";
        public string TargetPath { get; set; } = "";
        public bool CreateParentDirectory { get; set; }
    }
}
