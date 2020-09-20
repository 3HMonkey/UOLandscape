using ImGuiNET;
using UOLandscape.UI.Enums;
using Num = System.Numerics;

namespace UOLandscape.UI.Components
{
    internal sealed class InfoOverlayWindow : IInfoOverlayWindow
    {
        static Position _position = Position.TopRight;

        private bool _isActive;

        public bool IsActive => _isActive;

        public InfoOverlayWindow()
        {
            _isActive = true;
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
            const float DISTANCE = 10.0f;

            var io = ImGui.GetIO();
            if (_position != Position.Custom)
            {
                var viewport = ImGui.GetMainViewport();
                var workAreaPos = viewport.GetWorkPos();
                var workAreaSize = viewport.GetWorkSize();
                var windowPos = new Num.Vector2(
                    ((_position & Position.TopRight) == Position.TopRight)
                        ? (workAreaPos.X + workAreaSize.X - DISTANCE)
                        : (workAreaPos.X + DISTANCE),
                    ((_position & Position.BottomLeft) == Position.BottomLeft)
                        ? (workAreaPos.Y + workAreaSize.Y - DISTANCE)
                        : (workAreaPos.Y + DISTANCE));
                var windowPosPivot =
                    new Num.Vector2(((_position & Position.TopRight) == Position.TopRight) ? 1.0f : 0.0f,
                        ((_position & Position.BottomLeft) == Position.BottomLeft) ? 1.0f : 0.0f);
                ImGui.SetNextWindowPos(windowPos, ImGuiCond.Always, windowPosPivot);
                ImGui.SetNextWindowViewport(viewport.ID);
            }

            ImGui.SetNextWindowBgAlpha(0.35f); // Transparent background
            var window_flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoDocking |
                               ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings |
                               ImGuiWindowFlags.NoFocusOnAppearing | ImGuiWindowFlags.NoNav;
            if (_position != Position.Custom)
                window_flags |= ImGuiWindowFlags.NoMove;
            if (ImGui.Begin("Info Overlay", ref _isActive, window_flags))
            {
                ImGui.Text($"Application average \n{(int)(1000.0f / ImGui.GetIO().Framerate)} ms/frame ({(int)(ImGui.GetIO().Framerate)} FPS)");
                ImGui.Separator();
                ImGui.Text(ImGui.IsMousePosValid()
                    ? $"Mouse Position: ({io.MousePos.X},{io.MousePos.Y})"
                    : "Mouse Position: <invalid>");
                if (ImGui.BeginPopupContextWindow())
                {
                    if (ImGui.MenuItem("Custom", null, _position == Position.Custom)) _position = Position.Custom;
                    if (ImGui.MenuItem("Top-left", null, _position == Position.TopLeft)) _position = Position.TopLeft;
                    if (ImGui.MenuItem("Top-right", null, _position == Position.TopRight))
                    {
                        _position = Position.TopRight;
                    }
                    if (ImGui.MenuItem("Bottom-left", null, _position == Position.BottomLeft))
                    {
                        _position = Position.BottomLeft;
                    }
                    if (ImGui.MenuItem("Bottom-right", null, _position == Position.BottomRight))
                    {
                        _position = Position.BottomRight;
                    }
                    if (IsActive && ImGui.MenuItem("Close"))
                    {
                        Hide();
                    }

                    ImGui.EndPopup();
                }

                ImGui.End();
                return true;
            }

            return false;
        }
    }
}