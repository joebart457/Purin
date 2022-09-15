using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purin.PackageManager.Client.Models.Enums
{
    internal enum RunStatus: ushort
    {
        SUCCESS = 0,
        FAIL = 1,
        IGNORED = 2,
    }
}
