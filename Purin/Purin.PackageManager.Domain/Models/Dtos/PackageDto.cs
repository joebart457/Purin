using Purin.PackageManager.Domain.Models.Manifests;

namespace Purin.PackageManager.Domain.Models.Dtos
{
    public class PackageDto
    {
        public string? Name { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public PackageManifest? Data { get; set; }
    }
}
