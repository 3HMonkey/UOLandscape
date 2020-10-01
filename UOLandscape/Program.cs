using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UOLandscape.Client;
using UOLandscape.Configuration;
using UOLandscape.Native;
using UOLandscape.UI;
using UOLandscape.UI.Windows;

namespace UOLandscape
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            services.AddSingleton<ILogger>(logger);
            services.AddSingleton<IAppSettingsProvider, AppSettingsProvider>();
            services.AddSingleton<IConfigurationLoader, ConfigurationLoader>();
            services.AddSingleton<IConfigurationSaver, ConfigurationSaver>();
            services.AddSingleton<IAboutWindow, AboutWindow>();
            services.AddSingleton<IDockSpaceWindow, DockSpaceWindow>();
            services.AddSingleton<IInfoOverlayWindow, InfoOverlayWindow>();
            services.AddSingleton<INewProjectWindow, NewProjectWindow>();
            services.AddSingleton<ISettingsWindow, SettingsWindow>();
            services.AddSingleton<IToolsWindow, ToolsWindow>();
            services.AddSingleton<IDebugWindow, DebugWindow>();
            services.AddSingleton<IWindowService, WindowService>();
            services.AddSingleton<IClient, Client.Client>();
            services.AddSingleton<MainGame>();

            SetupDllPaths();

            Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");

            var serviceProvider = services.BuildServiceProvider();
            using var game = serviceProvider.GetService<MainGame>();
            game.Run();
        }

        private static void SetupDllPaths()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var libsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "libs");
            try
            {
                Kernel32.SetDefaultDllDirectories(Kernel32.LoadLibrarySearchDefaultDirs);
                Kernel32.AddDllDirectory(Path.Combine(libsDirectory, Environment.Is64BitProcess ? "x64" : "x86"));
            }
            catch
            {
                // Pre-Windows 7, KB2533623
                Kernel32.SetDllDirectory(Path.Combine(libsDirectory, Environment.Is64BitProcess ? "x64" : "x86"));
            }
        }
    }
}
