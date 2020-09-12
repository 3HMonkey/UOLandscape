using System;
using System.IO;
using System.Reflection;

namespace UOLandscape
{
    public static class UOLandscapeEnvironment
    {
        public static readonly bool IsUnix = Environment.OSVersion.Platform != PlatformID.Win32NT &&
                                             Environment.OSVersion.Platform != PlatformID.Win32Windows &&
                                             Environment.OSVersion.Platform != PlatformID.Win32S &&
                                             Environment.OSVersion.Platform != PlatformID.WinCE;

        public static readonly Version Version = Assembly.GetExecutingAssembly().GetName().Version;
        public static readonly string ExecutablePath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
    }
}
