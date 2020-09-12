
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UOLandscape.Configuration;
using UOLandscape.Utility.Logging;

namespace UOLandscape
{
    class Bootstrap
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDefaultDllDirectories(int directoryFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern void AddDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

        const int LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;


        public static GraphicsDevice GraphicsDevice;
        static void Main(string[] args)
        {


            //==========================================
            // Initialize the logger
            Log.Start(LogTypes.All);
            //==========================================
            // Load settings from json
            string globalSettingsPath = ConfigurationSettings.GetSettingsFilepath();

            if( (!Directory.Exists(Path.GetDirectoryName(globalSettingsPath)) ||
                                                       !File.Exists(globalSettingsPath)) )
            {
                // settings specified in path does not exists, make new one
                {
                    // TODO: 
                    ConfigurationSettings.GlobalSettings.Save();


                }
            }

            ConfigurationSettings.GlobalSettings = ConfigurationResolver.Load<ConfigurationSettings>(globalSettingsPath);

            // still invalid, cannot load settings
            if( ConfigurationSettings.GlobalSettings == null )
            {
                ConfigurationSettings.GlobalSettings = new ConfigurationSettings();
                ConfigurationSettings.GlobalSettings.Save();
            }

            if( !UOLandscapeEnvironment.IsUnix )
            {
                try
                {
                    SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS);
                    AddDllDirectory(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        Environment.Is64BitProcess ? "x64" : "x86"
                    ));
                }
                catch
                {
                    // Pre-Windows 7, KB2533623 
                    SetDllDirectory(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        Environment.Is64BitProcess ? "x64" : "x86"
                    ));
                }
            }

            Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");

            using( var mainApp = new Main() )
            {
                mainApp.Run();
                GraphicsDevice = mainApp.GraphicsDevice;
            }
        }
    }
}
