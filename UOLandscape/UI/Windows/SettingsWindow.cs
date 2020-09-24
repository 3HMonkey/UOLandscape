using ImGuiNET;
using UOLandscape.Client;
using UOLandscape.Configuration;

namespace UOLandscape.UI.Windows
{
    internal sealed class SettingsWindow : Window, ISettingsWindow
    {
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IClient _client;

        public SettingsWindow(
            IAppSettingsProvider appSettingsProvider,
            IClient client)
        {
            _isVisible = true;
            _appSettingsProvider = appSettingsProvider;
            _ultimaOnlinePath = _appSettingsProvider.AppSettings.UltimaOnlinePath;
            _client = client;
        }

        private string _ultimaOnlinePath;

        public override bool Show(uint dockSpaceId)
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(500, 130));
            if (ImGui.Begin("Settings", ref _isVisible, ImGuiWindowFlags.NoResize))
            {
                ImGui.TextUnformatted("Ultima Online Path");
                var ultimaOnlinePath = _ultimaOnlinePath;
                if (ImGui.InputText("##PathBox", ref ultimaOnlinePath, 128))
                {
                    _ultimaOnlinePath = ultimaOnlinePath;
                }

                if (!string.IsNullOrEmpty(_ultimaOnlinePath))
                {
                    ImGui.TextUnformatted(_ultimaOnlinePath);
                }
                else
                {
                    ImGui.Text("*");
                }

                if (ImGui.Button("Save"))
                {
                    _appSettingsProvider.AppSettings.UltimaOnlinePath = _ultimaOnlinePath;
                    _appSettingsProvider.Save();
                }

                ImGui.SameLine();
                if (ImGui.Button("Initialize"))
                {
                    _client.Load();
                    Hide();
                }

                ImGui.End();
                return true;
            }

            return false;
        }
    }
}