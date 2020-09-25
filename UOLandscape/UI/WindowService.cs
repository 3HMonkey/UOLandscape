using UOLandscape.UI.Windows;

namespace UOLandscape.UI
{
    internal sealed class WindowService : IWindowService
    {
        public ISettingsWindow SettingsWindow { get; }
        public IDockSpaceWindow DockSpaceWindow { get; }
        public INewProjectWindow NewProjectWindow { get; }
        public IToolsWindow ToolsWindow { get; }
        public IInfoOverlayWindow InfoOverlayWindow { get; }
        public IAboutWindow AboutWindow { get; }
        public IDebugWindow DebugWindow { get; }

        public WindowService(
            ISettingsWindow settingsWindow,
            IDockSpaceWindow dockSpaceWindow,
            INewProjectWindow newProjectWindow,
            IToolsWindow toolsWindow,
            IInfoOverlayWindow infoOverlayWindow,
            IAboutWindow aboutWindow,
            IDebugWindow debugWindow)
        {
            SettingsWindow = settingsWindow;
            DockSpaceWindow = dockSpaceWindow;
            NewProjectWindow = newProjectWindow;
            ToolsWindow = toolsWindow;
            InfoOverlayWindow = infoOverlayWindow;
            AboutWindow = aboutWindow;
            DebugWindow = debugWindow;
        }
    }
}