using ImGuiNET;
using Num = System.Numerics;

namespace UOLandscape.UI.Windows
{
    internal sealed class DockSpaceWindow : Window, IDockSpaceWindow
    {
        static bool _optFullscreen = true;
        static bool _optPadding = false;
        static ImGuiDockNodeFlags _dockspaceFlags = ImGuiDockNodeFlags.None;

        public DockSpaceWindow()
        {
            _isVisible = true;
        }

        public override bool Show(uint dockSpaceId)
        {
            var windowFlags = ImGuiWindowFlags.NoDocking;
            if (_optFullscreen)
            {
                var viewport = ImGui.GetMainViewport();
                ImGui.SetNextWindowPos(viewport.GetWorkPos());
                ImGui.SetNextWindowSize(viewport.GetWorkSize());
                ImGui.SetNextWindowViewport(viewport.ID);
                ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
                windowFlags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize |
                                ImGuiWindowFlags.NoMove;
                windowFlags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
            }
            else
            {
                _dockspaceFlags &= ~ImGuiDockNodeFlags.PassthruCentralNode;
            }

            // When using ImGuiDockNodeFlags_PassthruCentralNode, DockSpace() will render our background
            // and handle the pass-thru hole, so we ask Begin() to not render a background.
            if ((_dockspaceFlags & ImGuiDockNodeFlags.PassthruCentralNode) == ImGuiDockNodeFlags.PassthruCentralNode)
            {
                windowFlags |= ImGuiWindowFlags.NoBackground;
            }
            // Important: note that we proceed even if Begin() returns false (aka window is collapsed).
            // This is because we want to keep our DockSpace() active. If a DockSpace() is inactive,
            // all active windows docked into it will lose their parent and become undocked.
            // We cannot preserve the docking relationship between an active window and an inactive docking, otherwise
            // any change of dockspace/settings would lead to windows being stuck in limbo and never being visible.
            if (!_optPadding)
            {
                ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Num.Vector2(0.0f, 0.0f));
            }

            ImGui.Begin("DockSpace", ref _isVisible, windowFlags);
            if (!_optPadding)
            {
                ImGui.PopStyleVar();
            }

            if (_optFullscreen)
            {
                ImGui.PopStyleVar(2);
            }

            // DockSpace
            var io = ImGui.GetIO();
            if ((io.ConfigFlags & ImGuiConfigFlags.DockingEnable) == ImGuiConfigFlags.DockingEnable)
            {
                var dockspaceId = ImGui.GetID("MyDockSpace");
                ImGui.DockSpace(dockspaceId, new Num.Vector2(0.0f, 0.0f), _dockspaceFlags);
            }

            return true;
        }
    }
}