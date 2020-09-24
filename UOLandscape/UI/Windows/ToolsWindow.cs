using ImGuiNET;

namespace UOLandscape.UI.Windows
{
    internal sealed class ToolsWindow : Window, IToolsWindow
    {
        public ToolsWindow()
        {
            _isVisible = true;
        }

        public override bool Show(uint dockSpaceId)
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(100, 450));

            if (ImGui.Begin("Tools", ref _isVisible, ImGuiWindowFlags.NoResize))
            {
                ImGui.End();
                return true;
            }

            return false;
        }
    }
}