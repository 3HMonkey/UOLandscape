using SDL2;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using UOLandscape.Configuration;

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

        public Client(ILogger<Client> logger, IAppSettingsProvider appSettingsProvider)
        {
            _logger = logger;
            _appSettingsProvider = appSettingsProvider;
        }

        public void Load()
        {
            _logger.LogTrace(">>>>>>>>>>>>> Loading >>>>>>>>>>>>>");

            var clientPath = _appSettingsProvider.AppSettings.UltimaOnlinePath;
            _logger.LogTrace($"Ultima Online installation folder: {clientPath}");

            _logger.LogTrace("Loading files...");

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
                _logger.LogError("Invalid client directory: " + clientPath);
                ShowErrorMessage($"'{clientPath}' is not a valid UO directory");
                throw new InvalidClientDirectory($"'{clientPath}' is not a valid directory");
            }

            // try to load the client version
            if (!ClientVersionHelper.IsClientVersionValid(clientVersionText, out ClientVersion clientVersion))
            {
                _logger.LogWarning(
                    $"Client version [{clientVersionText}] is invalid, let's try to read the client.exe");

                // mmm something bad happened, try to load from client.exe
                if (!ClientVersionHelper.TryParseFromFile(Path.Combine(clientPath, "client.exe"), out clientVersionText) ||
                    !ClientVersionHelper.IsClientVersionValid(clientVersionText, out clientVersion))
                {
                    _logger.LogError("Invalid client version: " + clientVersionText);
                    ShowErrorMessage($"Impossible to define the client version.\nClient version: '{clientVersionText}'");
                    throw new InvalidClientVersion($"Invalid client version: '{clientVersionText}'");
                }

                _logger.LogTrace($"Found a valid client.exe [{clientVersionText} - {clientVersion}]");

                // update the wrong/missing client version in settings.json
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

            _logger.LogTrace($"Client path: '{clientPath}'");
            _logger.LogTrace($"Client version: {clientVersion}");
            _logger.LogTrace($"Protocol: {Protocol}");
            _logger.LogTrace($"UOP? {(IsUOPInstallation ? "yes" : "no")}");

            // ok now load uo files
            //UOFileManager.Load();
            //StaticFilters.Load();

            _logger.LogTrace(">>>>>>>>>>>>> DONE >>>>>>>>>>>>>");
        }
    }
}