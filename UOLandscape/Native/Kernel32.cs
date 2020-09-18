using System.Runtime.InteropServices;

namespace UOLandscape.Native
{
    internal static class Kernel32
    {
        private const string DllName = "kernel32.dll";
        internal const int LoadLibrarySearchDefaultDirs = 0x00001000;

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetDefaultDllDirectories(int directoryFlags);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern void AddDllDirectory(string lpPathName);

        [DllImport(DllName, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetDllDirectory(string lpPathName);
    }
}