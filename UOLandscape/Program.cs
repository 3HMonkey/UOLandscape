using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using UOLandscape.Client;
using UOLandscape.Configuration;
using UOLandscape.Native;
using UOLandscape.UI;
using UOLandscape.UI.Windows;

namespace UOLandscape
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole(consoleOptions =>
                {
                    consoleOptions.Format = ConsoleLoggerFormat.Systemd;
                });
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            });

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

            Environment.SetEnvironmentVariable("FNA3D_OPENGL_FORCE_CORE_PROFILE", "1");
            Environment.SetEnvironmentVariable("FNA3D_FORCE_DRIVER", "OpenGL");

            var serviceProvider = services.BuildServiceProvider();
            var game = serviceProvider.GetService<MainGame>();
            game.Run();
        }

        private static void Configure(ILoggingBuilder obj)
        {
            throw new NotImplementedException();
        }

        private static void SetupDllPaths()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            try
            {
                Kernel32.SetDefaultDllDirectories(Kernel32.LoadLibrarySearchDefaultDirs);
                Kernel32.AddDllDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    Environment.Is64BitProcess ? "x64" : "x86"
                ));
            }
            catch
            {
                // Pre-Windows 7, KB2533623
                Kernel32.SetDllDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    Environment.Is64BitProcess ? "x64" : "x86"));
            }
        }
    }
}
