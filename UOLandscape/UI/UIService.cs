namespace UOLandscape.UI
{
    internal sealed class UIService : IUIService
    {
        public ISettingsWindow SettingsWindow { get; }
        public IDockSpaceWindow DockSpaceWindow { get; }
        public INewProjectWindow NewProjectWindow { get; }
        public IToolsWindow ToolsWindow { get; }
        public IInfoOverlayWindow InfoOverlayWindow { get; }

        public IAboutWindow AboutWindow { get; }

        public UIService(
            ISettingsWindow settingsWindow,
            IDockSpaceWindow dockSpaceWindow,
            INewProjectWindow newProjectWindow,
            IToolsWindow toolsWindow,
            IInfoOverlayWindow infoOverlayWindow,
            IAboutWindow aboutWindow)
        {
            SettingsWindow = settingsWindow;
            DockSpaceWindow = dockSpaceWindow;
            NewProjectWindow = newProjectWindow;
            ToolsWindow = toolsWindow;
            InfoOverlayWindow = infoOverlayWindow;
            AboutWindow = aboutWindow;
        }
    }
}