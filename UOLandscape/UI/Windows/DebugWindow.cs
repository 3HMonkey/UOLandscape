using System.Collections.Generic;
using ImGuiNET;

namespace UOLandscape.UI.Windows
{
    internal sealed class DebugWindow : Window, IDebugWindow
    {
        private bool _autoScroll;
        private List<string> _debugListBuffer;

        public List<string> Entries => _debugListBuffer;

        public bool AutoScroll => _autoScroll;

        public DebugWindow()
        {
            _debugListBuffer = new List<string>();
            _isVisible = true;
            for (int i = 0; i < 30; i++)
            {
                _debugListBuffer.Add(
                    "This is a test text This is a test text This is a test text This is a test text This is a test text This is a test text");
            }
        }

        public void Clear()
        {
            _debugListBuffer.Clear();
        }

        public void Add(string newEntry)
        {
            _debugListBuffer.Add(newEntry);
        }

        public override bool Show(uint dockSpaceId)
        {
            //Initialize debug window
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(450, 200));
            if (!ImGui.Begin("Debug", ref _isVisible))
            {
                ImGui.End();
                return false;
            }

            // Adds options to popup menu
            if (ImGui.BeginPopup("Options"))
            {
                ImGui.Checkbox("Auto-scroll", ref _autoScroll);
                ImGui.EndPopup();
            }

            // Main window
            if (ImGui.Button("Options"))
            {
                ImGui.OpenPopup("Options");
            }

            ImGui.SameLine();
            var clear = ImGui.Button("Clear");
            ImGui.SameLine();
            var copy = ImGui.Button("Copy");
            ImGui.Separator();

            // Creates child component with scrolling
            ImGui.BeginChild("ScrollBox", new System.Numerics.Vector2(0, 0), true,
                ImGuiWindowFlags.HorizontalScrollbar);
            if (clear)
            {
                Clear();
            }

            if (copy)
            {
                ImGui.LogToClipboard();
            }

            foreach (var line in _debugListBuffer)
            {
                ImGui.TextUnformatted(line);
            }


            if (AutoScroll && ImGui.GetScrollY() >= ImGui.GetScrollMaxY())
            {
                ImGui.SetScrollHereY(1.0f);
            }

            ImGui.EndChild();
            ImGui.End();
            return true;
        }
    }
}