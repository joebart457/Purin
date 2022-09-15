using Purin.PackageManager.Client.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.PackageManager.Client.Models
{
    internal class InstallCommandResult
    {
        public RunStatus Status { get; set; }
        public int ExitCode { get; set; }
        public bool ProducedUsableExitCode { get; set; }
        public bool HadError { get; set; }
        public string? ErrorTrace { get; set; }
    }
}
