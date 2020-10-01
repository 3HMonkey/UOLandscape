using SDL2;
using System;
using System.IO;
using Serilog;
using UOLandscape.Configuration;
using UOLandscape.Artwork;

namespace UOLandscape.Client
{
    internal sealed class Client : IClient
    {
        private readonly ILogger _logger;
        private readonly IAppSettingsProvider _appSettingsProvider;
        public static ClientVersion Version { get; private set; }
        public static ClientFlags Protocol { get; set; }
        public static string ClientPath { get; private set; }
        public static bool IsUOPInstallation { get; private set; }

        public static bool UseUOPGumps { get; set; }

        public static void ShowErrorMessage(string msg)
        {
            SDL.SDL_ShowSimpleMessageBox(SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "ERROR", msg, IntPtr.Zero);
        }

        public Client(ILogger logger, IAppSettingsProvider appSettingsProvider)
        {
            _logger = logger;
            _appSettingsProvider = appSettingsProvider;
        }


        public void Load()
        {
            _logger.Debug(">>>>>>>>>>>>> Loading >>>>>>>>>>>>>");

            var clientPath = _appSettingsProvider.AppSettings.UltimaOnlinePath;
            _logger.Debug($"Ultima Online installation folder: {clientPath}");

            _logger.Debug("Loading files...");

            if (!string.IsNullOrWhiteSpace(_appSettingsProvider.AppSettings.ClientVersion))
            {
                // sanitize client version
                _appSettingsProvider.AppSettings.ClientVersion = _appSettingsProvider.AppSettings.ClientVersion
                    .Replace(",", ".")
                    .Replace(" ", string.Empty)
                    .ToLower();
            }

            var clientVersionText = _appSettingsProvider.AppSettings.ClientVersion;

            // check if directory is good
            if (!Directory.Exists(clientPath))
            {
                _logger.Error("Invalid client directory: " + clientPath);
                ShowErrorMessage($"'{clientPath}' is not a valid UO directory");
                throw new InvalidClientDirectory($"'{clientPath}' is not a valid directory");
            }

            // try to load the client version
            if (!ClientVersionHelper.IsClientVersionValid(clientVersionText, out ClientVersion clientVersion))
            {
                _logger.Warning(
                    $"Client version [{clientVersionText}] is invalid, let's try to read the client.exe");

                if (!ClientVersionHelper.TryParseFromFile(Path.Combine(clientPath, "client.exe"), out clientVersionText) ||
                    !ClientVersionHelper.IsClientVersionValid(clientVersionText, out clientVersion))
                {
                    _logger.Error("Invalid client version: " + clientVersionText);
                    ShowErrorMessage($"Impossible to define the client version.\nClient version: '{clientVersionText}'");
                    throw new InvalidClientVersion($"Invalid client version: '{clientVersionText}'");
                }

                _logger.Debug($"Found a valid client.exe [{clientVersionText} - {clientVersion}]");

                _appSettingsProvider.AppSettings.ClientVersion = clientVersionText;
            }

            Version = clientVersion;
            ClientPath = clientPath;
            IsUOPInstallation = Version >= ClientVersion.CV_7000; //&& File.Exists(UOFileManager.GetUOFilePath("MainMisc.uop"));
            Protocol = ClientFlags.CF_T2A;

            if (Version >= ClientVersion.CV_200)
            {
                Protocol |= ClientFlags.CF_RE;
            }

            if (Version >= ClientVersion.CV_300)
            {
                Protocol |= ClientFlags.CF_TD;
            }

            if (Version >= ClientVersion.CV_308)
            {
                Protocol |= ClientFlags.CF_LBR;
            }

            if (Version >= ClientVersion.CV_308Z)
            {
                Protocol |= ClientFlags.CF_AOS;
            }

            if (Version >= ClientVersion.CV_405A)
            {
                Protocol |= ClientFlags.CF_SE;
            }

            if (Version >= ClientVersion.CV_60144)
            {
                Protocol |= ClientFlags.CF_SA;
            }

            _logger.Debug($"Client path: '{clientPath}'");
            _logger.Debug($"Client version: {clientVersion}");
            _logger.Debug($"Protocol: {Protocol}");
            _logger.Debug($"UOP? {(IsUOPInstallation ? "yes" : "no")}");

            // ok now load uo files
            //UOFileManager.Load();
            //StaticFilters.Load();
            new ArtworkProvider(_logger, IsUOPInstallation, clientPath);

            _logger.Debug(">>>>>>>>>>>>> DONE >>>>>>>>>>>>>");
        }
    }
}