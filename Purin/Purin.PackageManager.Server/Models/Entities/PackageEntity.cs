using Newtonsoft.Json;
using Purin.PackageManager.Domain.Models.Dtos;
using Purin.PackageManager.Domain.Models.Manifests;
using Sql;

namespace Purin.PackageManager.Server.Models.Entities
{
    public class PackageEntity
    {
        [Identity]
        [AutoIncrement]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Data { get; set; }

        public PackageDto ToDto()
        {
            var packageDto = new PackageDto();
            packageDto.Name = this.Name;
            packageDto.Description = this.Description;
            packageDto.Tag = this.Tag;
            packageDto.TimeStamp = this.TimeStamp;
            packageDto.Data = JsonConvert.DeserializeObject<PackageManifest>(this.Data ?? "");
            return packageDto;
        }

        public static PackageEntity FromDto(AddPackageDto dto)
        {
            var entity = new PackageEntity();
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Tag = dto.Tag;
            entity.TimeStamp = DateTime.Now;
            entity.Data = JsonConvert.SerializeObject(dto.Data);
            return entity;
        }

        public static PackageEntity FromDto(UpdatePackageDto dto)
        {
            var entity = new PackageEntity();
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Tag = dto.Tag;
            entity.TimeStamp = DateTime.Now;
            entity.Data = JsonConvert.SerializeObject(dto.Data);
            return entity;
        }

    }
}
