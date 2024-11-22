using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacioli.WindowsApp.NET8.Util
{
    public class VersionInformation
    {
        public static string GetVersion()
        {
            Version? version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return $"v{version!.Major}.{version.Minor}.{version.Build}";
        }
    }
}
