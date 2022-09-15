using Purin.PackageManager.Domain.Models.Manifests;

namespace Purin.PackageManager.Domain.Models.Dtos
{
    public class AddPackageDto
    {
        public string? Name { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public PackageManifest? Data { get; set; }
    }
}
