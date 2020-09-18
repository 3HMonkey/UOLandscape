using ImGuiNET;
using Microsoft.Extensions.Logging;
using UOLandscape.Client;
using UOLandscape.Configuration;

namespace UOLandscape.UI.Components
{
    internal sealed class SettingsWindow : ISettingsWindow
    {
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IClient _client;

        private bool _isActive;

        public bool IsActive => _isActive;

        public SettingsWindow(
            IAppSettingsProvider appSettingsProvider,
            IClient client)
        {
            _appSettingsProvider = appSettingsProvider;
            _client = client;
        }

        public void Hide()
        {
            _isActive = false;
        }

        public void ToggleActive()
        {
            _isActive = !_isActive;
        }

        public bool Show(uint dockSpaceId)
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(500, 130));
            if (ImGui.Begin("Settings", ref _isActive, ImGuiWindowFlags.NoResize))
            {
                ImGui.TextUnformatted("Ultima Online Path");
                var ultimaOnlinePath = _appSettingsProvider.AppSettings.UltimaOnlinePath;
                ImGui.InputText("##PathBox", ref ultimaOnlinePath, 100);

                if (string.IsNullOrEmpty(_appSettingsProvider.AppSettings.UltimaOnlinePath))
                {
                    ImGui.TextUnformatted(_appSettingsProvider.AppSettings.UltimaOnlinePath);
                }
                else
                {
                    ImGui.Text("*");
                }

                if (ImGui.Button("Save"))
                {
                    _appSettingsProvider.AppSettings.UltimaOnlinePath = ultimaOnlinePath;
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