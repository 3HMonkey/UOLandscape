using ImGuiNET;
using UOLandscape.Configuration;

namespace UOLandscape.UI.Windows
{
    internal sealed class NewProjectWindow : Window, INewProjectWindow
    {
        private readonly IAppSettingsProvider _appSettingsProvider;

        public NewProjectWindow(IAppSettingsProvider appSettingsProvider)
        {
            _appSettingsProvider = appSettingsProvider;
        }

        public override bool Show(uint dockspaceId)
        {
            ImGui.SetNextWindowDockID(dockspaceId, ImGuiCond.FirstUseEver);

            if (ImGui.Begin("Settings", ref _isVisible))
            {
                ImGui.TextUnformatted("Ultima Online Path");

                var ultimaOnlinePath = _appSettingsProvider.AppSettings.UltimaOnlinePath;
                ImGui.InputText("##PathBox", ref ultimaOnlinePath, 128);
                _appSettingsProvider.AppSettings.UltimaOnlinePath = ultimaOnlinePath;

                if(string.IsNullOrEmpty(_appSettingsProvider.AppSettings.UltimaOnlinePath))
                {
                    ImGui.TextUnformatted(_appSettingsProvider.AppSettings.UltimaOnlinePath);
                }
                else
                {
                    ImGui.Text("*");
                }

                ImGui.End();
                return true;
            }
            return false;
        }
    }

}

