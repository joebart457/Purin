

using Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Purin.PackageManager.Server.Contexts;
using Purin.PackageManager.Domain.Models.Dtos;
using Purin.PackageManager.Server.Models.Entities;
using Purin.PackageManager.Domain.Models.Manifests;
using Purin.PackageManager.Server.Services;

namespace Purin.PackageManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackagesController : ControllerBase
    {
        public PackagesController()
        {
        }

        [HttpGet]
        public ActionResult<IEnumerable<PackageDto>> GetAll()
        {
            try
            {
                var packages = DataContext.Connection.Select<PackageEntity>()
                    .Select(e => e.ToDto())
                    .Where(e => e.Data != null);


                return Ok(packages);
            }
            catch (Exception ex)
            {
                CliLogger.LogError(ex.ToString());
                return UnprocessableEntity();
            }
        }

        [HttpGet]
        [Route("{packageName}")]
        public ActionResult<IEnumerable<PackageDto>> GetAllVersions([FromRoute] string packageName)
        {
            try
            {
                var packages = DataContext.Connection.Select<PackageEntity>()
                    .Where(e => e.Name == packageName)
                    .Select(e => e.ToDto())
                    .Where(e => e.Data != null);


                return Ok(packages);
            }
            catch (Exception ex)
            {
                CliLogger.LogError(ex.ToString());
                return UnprocessableEntity();
            }
        }

        [HttpGet]
        [Route("{packageName}/latest")]
        public ActionResult<IEnumerable<PackageDto>> GetLatestVersion([FromRoute] string packageName)
        {
            try
            {
                var package = DataContext.Connection.Select<PackageEntity>()
                    .Where(e => e.Name == packageName)
                    .Select(e => e.ToDto())
                    .Where(e => e.Data != null)
                    .OrderBy(e => e.TimeStamp)
                    .FirstOrDefault();

                if (package == null) return NotFound();
                return Ok(package);
            }
            catch (Exception ex)
            {
                CliLogger.LogError(ex.ToString());
                return UnprocessableEntity();
            }
        }

        [HttpGet]
        [Route("{packageName}/tag/{packageTag}")]
        public ActionResult<IEnumerable<PackageDto>> GetTagged([FromRoute] string packageName, [FromRoute] string packageTag)
        {
            try
            {
                var package = DataContext.Connection.Select<PackageEntity>()
                    .Where(e => e.Name == packageName && e.Tag == packageTag)
                    .Select(e => e.ToDto())
                    .Where(e => e.Data != null)
                    .OrderBy(e => e.TimeStamp)
                    .FirstOrDefault();

                if (package == null) return NotFound();
                return Ok(package);
            }
            catch (Exception ex)
            {
                CliLogger.LogError(ex.ToString());
                return UnprocessableEntity();
            }
        }

        [HttpPost]
        public ActionResult PostPackage([FromBody] AddPackageDto package)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(package.Name))
                    throw new ArgumentNullException(nameof(package.Name));
                if (string.IsNullOrWhiteSpace(package.Tag))
                    throw new ArgumentNullException(nameof(package.Tag));
                if (package.Data == null)
                    throw new ArgumentNullException(nameof(package.Data));

                if (DataContext.Connection.Select<PackageEntity>().Any(e => e.Name == package.Name && e.Tag == package.Tag))
                    return Conflict($"package named {package.Name}:{package.Tag} already exists");
                DataContext.Connection.Insert(PackageEntity.FromDto(package));
                return Ok();
            }
            catch (Exception ex)
            {
                CliLogger.LogError(ex.ToString());
                return UnprocessableEntity();
            }
        }

        [HttpPut]
        public ActionResult UpdatePackage([FromBody] UpdatePackageDto package)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(package.Name))
                    throw new ArgumentNullException(nameof(package.Name));
                if (string.IsNullOrWhiteSpace(package.Tag))
                    throw new ArgumentNullException(nameof(package.Tag));
                if (package.Data == null)
                    throw new ArgumentNullException(nameof(package.Data));

                var count = DataContext.Connection.Select<PackageEntity>().Count(e => e.Name == package.Name && e.Tag == package.Tag);
                if (count > 1)
                    return Conflict($"unable to resolve name. More than one package named {package.Name}:{package.Tag} exists");
                if (count == 0) return NotFound();
                else
                {
                    DataContext.Connection.Update(PackageEntity.FromDto(package));
                }
                return Ok();
            }
            catch (Exception ex)
            {
                CliLogger.LogError(ex.ToString());
                return UnprocessableEntity();
            }
        }

        [HttpDelete]
        [Route("{packageName}/tag/{tag}")]
        public ActionResult DeletePackage([FromRoute] string packageName, [FromRoute] string packageTag)
        {
            try
            {
                var package = DataContext.Connection.Select<PackageEntity>()
                    .Where(e => e.Name == packageName && e.Tag == packageTag)
                    .FirstOrDefault();

                if (package == null) return NotFound();
                DataContext.Connection.Delete(package);
                return Ok(package);
            }
            catch (Exception ex)
            {
                CliLogger.LogError(ex.ToString());
                return UnprocessableEntity();
            }
        }
    }
}
