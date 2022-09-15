using CliParser;
using Logger;
using Newtonsoft.Json;
using Purin.PackageManager.Client.Models;
using Purin.PackageManager.Client.Models.Enums;
using Purin.PackageManager.Domain.Models.Dtos;
using Purin.PackageManager.Domain.Models.Manifests;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using Web;

namespace Purin.PackageManager.Client.Services
{
    [Entry("client")]
    internal static class ProgramStartupService
    {
        [Command("switch")]
        [SubCommand("remote")]
        public static void SwitchRemote(string hostName)
        {
            ConfigService.Settings.HostName = hostName;
            CliLogger.LogInfo($"remote updated to {ConfigService.Settings.HostName}");
        }

        [Command("ls")]
        [SubCommand("packages")]
        public static async Task GetAllPackages()
        {
            var packages = await WebClient.GetAsync<List<PackageDto>>(GetRoute("/Packages"));
            CliLogger.Log(packages);
        }

        [Command("ls")]
        [SubCommand("package")]
        public static async Task GetPackage(string name, [Option] string tag  = "")
        {
            var route = tag == "" ? $"/Packages/{name}" : (tag == "latest"? $"/Packages/{name}/latest" : $"/Packages/{name}/tag/{tag}");

            var package = await WebClient.GetAsync<PackageDto>(GetRoute(route));
            CliLogger.Log(new List<PackageDto> { package });
        }

        [Command("del")]
        [SubCommand("package")]
        public static async Task DeletePackage(string name, string tag)
        {
            var route = $"/Packages/{name}/tag/{tag}";

            var response = await WebClient.DeleteAsync(GetRoute(route));
            if (response.IsSuccessStatusCode)
            {
                CliLogger.LogSuccess($"deleted package {name}:{tag}");
            }
            else
            {
                CliLogger.LogWarning($"response code {response.StatusCode}: {response.Content}");
            }
        }

        [Command("add")]
        [SubCommand("package")]
        public static async Task AddPackage(string path)
        {
            var packageManifest = GetPackageManifest(path);
            var dto = CreateAddDtoFromManifest(packageManifest);

            var response = await WebClient.PostAsync(GetRoute($"/Packages"), dto);
            if (response.IsSuccessStatusCode)
            {
                CliLogger.LogSuccess($"added package {dto.Name}:{dto.Tag}");
            }
            else
            {
                CliLogger.LogWarning($"response code {response.StatusCode}: {response.Content}");
            }
        }

        [Command("update")]
        [SubCommand("package")]
        public static async Task UpdatePackage(string path)
        {
            var packageManifest = GetPackageManifest(path);
            var dto = CreateDtoFromManifest(packageManifest);

            var response = await WebClient.PutAsync(GetRoute($"/Packages"), dto);
            if (response.IsSuccessStatusCode)
            {
                CliLogger.LogSuccess($"updated package {dto.Name}:{dto.Tag}");
            }
            else
            {
                CliLogger.LogWarning($"response code {response.StatusCode}: {response.Content}");
            }
        }

        [Command("install")]
        public static async Task InstallPackage(string name, string tag = "")
        {
            var route = string.IsNullOrWhiteSpace(tag) ? $"/Packages/{name}/latest" : $"/Packages/{name}/tag/{tag}";
            var packageManifest = await WebClient.GetAsync<PackageManifest>(GetRoute(route));

            if (packageManifest == null) throw new Exception("unable to retrieve package manifest from remote");

            CliLogger.LogInfo($".installing package {packageManifest.Name}:{packageManifest.Tag}");
            CliLogger.LogInfo($"..downloading assets");
            if (packageManifest.DownloadDataManifest == null)
            {
                CliLogger.LogWarning("..skipped (no assests to download)");
            } 
            else await Download(packageManifest.DownloadDataManifest);

            CliLogger.Log("..finshed downloading");
            CliLogger.Log(".extracting archives");
            if (packageManifest.ArchivesToExtract?.Any() == true)
            {
                foreach (var manifest in packageManifest.ArchivesToExtract)
                {
                    ExtractArchive(manifest);
                }
                CliLogger.LogInfo("..finished unzipping");
            } 
            else CliLogger.LogWarning("..skipped (found no files to unzip)");
            
            CliLogger.LogInfo(".running install commands");
            if (packageManifest.InstallCommands?.Any() == true)
            {
                var orderedRunManifests = packageManifest.InstallCommands.OrderBy(r => r.Stage);
                foreach (var runManifest in orderedRunManifests)
                {
                    CliLogger.LogInfo($"..({runManifest.Stage}/{orderedRunManifests.Count()}) run ~$ {runManifest.Cmd}");
                    var rc = RunInstallCommand(runManifest);
                    if (rc.Status == RunStatus.SUCCESS || rc.Status == RunStatus.IGNORED)
                    {
                        CliLogger.LogSuccess($".. SUCCESS ({(rc.ProducedUsableExitCode ? $"{rc.ExitCode}" : "")})");
                        continue;
                    }
                    CliLogger.LogError($"..FAIL -- {(rc.ProducedUsableExitCode ? $"exit code ({rc.ExitCode})" : "")}.");
                    CliLogger.LogError($"..FAILURE REASON -- {rc.ErrorTrace}");
                    return;
                }
            }
            else CliLogger.LogWarning("..skipped (found no commands to run)");
            
            CliLogger.LogSuccess($".package {packageManifest.Name}:{packageManifest.Tag} installed");
        }

        #region Helpers
        private static async Task Download(DownloadDataManifest manifest)
        {
            if (string.IsNullOrWhiteSpace(manifest.Uri)) throw new ArgumentNullException(manifest.Uri);
            if (!Directory.Exists(Path.GetDirectoryName(manifest.TargetPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(manifest.TargetPath) ?? "");
            }
            if (!manifest.Remote)
            {
                if (!File.Exists(manifest.Uri)) throw new FileNotFoundException(manifest.Uri);
                File.Copy(manifest.Uri, manifest.TargetPath, true);
            }
            else
            {
                await WebClient.DownloadAsync(manifest.Uri, manifest.TargetPath);
            }
        }
        private static void ExtractArchive(ExtractArchiveManifest manifest)
        {
            if (!File.Exists(manifest.SourcePath)) throw new FileNotFoundException(manifest.SourcePath);
            if (!Directory.Exists(manifest.TargetPath))
            {
                if (!manifest.CreateParentDirectory) throw new DirectoryNotFoundException(manifest.TargetPath);
                Directory.CreateDirectory(manifest.TargetPath);
            }
            ZipFile.ExtractToDirectory(manifest.SourcePath, manifest.TargetPath, true);
        }

        private static InstallCommandResult RunInstallCommand(InstallCommandManifest manifest)
        {
            try
            {
                var argsPrepend = "-c";
                var shellName = "/bin/bash";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    shellName = "cmd";
                    argsPrepend = "/c ";
                }
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = shellName,
                    Arguments = $"{argsPrepend} {manifest.Cmd}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                };
                Process proc = new Process()
                {
                    StartInfo = startInfo,
                };
                proc.Start();
                proc.WaitForExit();
                var stdOut = proc.StandardOutput.ReadToEnd().Split('\n');
                foreach (var line in stdOut)
                {
                    CliLogger.LogLogs($"....{line}");
                }
                var stdErr = proc.StandardError.ReadToEnd().Split('\n');
                foreach (var line in stdErr)
                {
                    CliLogger.LogWarning($".e..{line}");
                }

                return new InstallCommandResult
                {
                    ExitCode = proc.ExitCode,
                    ProducedUsableExitCode = true,
                    Status = manifest.ExitCodeSuccess.Contains(proc.ExitCode) ? RunStatus.SUCCESS : (manifest.IgnoreOnFail ? RunStatus.IGNORED : RunStatus.FAIL),
                    HadError = false,
                };
            }
            catch (Exception ex)
            {
                return new InstallCommandResult
                {
                    ProducedUsableExitCode = false,
                    Status = manifest.IgnoreOnFail ? RunStatus.IGNORED : RunStatus.FAIL,
                    HadError = true,
                    ErrorTrace = ex.ToString(),
                };
            }
        }

        private static string GetRoute(string uri)
        {
            return $"{ConfigService.Settings.HostName}{uri}";
        }

        private static PackageDto CreateDtoFromManifest(PackageManifest manifest)
        {
            PackageDto packageDto = new PackageDto();
            packageDto.Name = manifest.Name;
            packageDto.Description = manifest.Description;
            packageDto.Tag = manifest.Tag;
            packageDto.Data = manifest;
            return packageDto;
        }

        private static AddPackageDto CreateAddDtoFromManifest(PackageManifest manifest)
        {
            AddPackageDto packageDto = new AddPackageDto();
            packageDto.Name = manifest.Name;
            packageDto.Description = manifest.Description;
            packageDto.Tag = manifest.Tag;
            packageDto.Data = manifest;
            return packageDto;
        }

        private static PackageManifest GetPackageManifest(string path)
        {
            var packageManifest = JsonConvert.DeserializeObject<PackageManifest>(File.ReadAllText(path));
            if (packageManifest == null) throw new Exception($"unable to convert object: invalid json at {path}");
            return packageManifest;
        }

        #endregion

    }
}
