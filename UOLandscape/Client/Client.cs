using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UOLandscape.Configuration;
using UOLandscape.Utility.Logging;

namespace UOLandscape.Client
{
    static class Client
    {
        public static ClientVersion Version { get; private set; }
        public static ClientFlags Protocol { get; set; }
        public static string ClientPath { get; private set; }
        public static bool IsUOPInstallation { get; private set; }
        public static bool UseUOPGumps { get; set; }
                       

        public static void ShowErrorMessage(string msg)
        {
            SDL.SDL_ShowSimpleMessageBox(SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR, "ERROR", msg, IntPtr.Zero);
        }


        public static void Load()
        {
            Log.Trace(">>>>>>>>>>>>> Loading >>>>>>>>>>>>>");

            string clientPath = ConfigurationSettings.GlobalSettings.UOPath;
            Log.Trace($"Ultima Online installation folder: {clientPath}");

            Log.Trace("Loading files...");

            if( !string.IsNullOrWhiteSpace(ConfigurationSettings.GlobalSettings.ClientVersion) )
            {
                // sanitize client version
                ConfigurationSettings.GlobalSettings.ClientVersion = ConfigurationSettings.GlobalSettings.ClientVersion.Replace(",", ".").Replace(" ", "").ToLower();
            }

            string clientVersionText = ConfigurationSettings.GlobalSettings.ClientVersion;

            // check if directory is good
            if( !Directory.Exists(clientPath) )
            {
                Log.Error("Invalid client directory: " + clientPath);
                ShowErrorMessage($"'{clientPath}' is not a valid UO directory");
                throw new InvalidClientDirectory($"'{clientPath}' is not a valid directory");
            }

            // try to load the client version
            if( !ClientVersionHelper.IsClientVersionValid(clientVersionText, out ClientVersion clientVersion) )
            {
                Log.Warn($"Client version [{clientVersionText}] is invalid, let's try to read the client.exe");

                // mmm something bad happened, try to load from client.exe
                if( !ClientVersionHelper.TryParseFromFile(Path.Combine(clientPath, "client.exe"), out clientVersionText) ||
                    !ClientVersionHelper.IsClientVersionValid(clientVersionText, out clientVersion) )
                {
                    Log.Error("Invalid client version: " + clientVersionText);
                    ShowErrorMessage($"Impossible to define the client version.\nClient version: '{clientVersionText}'");
                    throw new InvalidClientVersion($"Invalid client version: '{clientVersionText}'");
                }

                Log.Trace($"Found a valid client.exe [{clientVersionText} - {clientVersion}]");

                // update the wrong/missing client version in settings.json
                ConfigurationSettings.GlobalSettings.ClientVersion = clientVersionText;
            }

            Version = clientVersion;
            ClientPath = clientPath;
            IsUOPInstallation = Version >= ClientVersion.CV_7000; //&& File.Exists(UOFileManager.GetUOFilePath("MainMisc.uop"));
            Protocol = ClientFlags.CF_T2A;

            if( Version >= ClientVersion.CV_200 )
                Protocol |= ClientFlags.CF_RE;
            if( Version >= ClientVersion.CV_300 )
                Protocol |= ClientFlags.CF_TD;
            if( Version >= ClientVersion.CV_308 )
                Protocol |= ClientFlags.CF_LBR;
            if( Version >= ClientVersion.CV_308Z )
                Protocol |= ClientFlags.CF_AOS;
            if( Version >= ClientVersion.CV_405A )
                Protocol |= ClientFlags.CF_SE;
            if( Version >= ClientVersion.CV_60144 )
                Protocol |= ClientFlags.CF_SA;

            Log.Trace($"Client path: '{clientPath}'");
            Log.Trace($"Client version: {clientVersion}");
            Log.Trace($"Protocol: {Protocol}");
            Log.Trace("UOP? " + (IsUOPInstallation ? "yes" : "no"));

            // ok now load uo files
            //UOFileManager.Load();
            //StaticFilters.Load();


            Log.Trace(">>>>>>>>>>>>> DONE >>>>>>>>>>>>>");
        }
    }
}
