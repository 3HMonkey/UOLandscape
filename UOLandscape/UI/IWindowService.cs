using UOLandscape.UI.Windows;

namespace UOLandscape.UI
{
    internal interface IWindowService
    {
        public ISettingsWindow SettingsWindow { get; }

        public IDockSpaceWindow DockSpaceWindow { get; }

        public INewProjectWindow NewProjectWindow { get; }

        public IToolsWindow ToolsWindow { get; }

        public IInfoOverlayWindow InfoOverlayWindow { get; }

        public IAboutWindow AboutWindow { get; }

        public IDebugWindow DebugWindow { get; }
    }
}