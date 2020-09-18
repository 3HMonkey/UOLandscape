namespace UOLandscape.UI
{
    internal sealed class UIService : IUIService
    {
        public ISettingsWindow SettingsWindow { get; }
        public IDockspaceWindow DockspaceWindow { get; }
        public INewProjectWindow NewProjectWindow { get; }
        public IToolsWindow ToolsWindow { get; }
        public IInfoOverlayWindow InfoOverlayWindow { get; }

        public IAboutWindow AboutWindow { get; }

        public UIService(
            ISettingsWindow settingsWindow,
            IDockspaceWindow dockspaceWindow,
            INewProjectWindow newProjectWindow,
            IToolsWindow toolsWindow,
            IInfoOverlayWindow infoOverlayWindow,
            IAboutWindow aboutWindow)
        {
            SettingsWindow = settingsWindow;
            DockspaceWindow = dockspaceWindow;
            NewProjectWindow = newProjectWindow;
            ToolsWindow = toolsWindow;
            InfoOverlayWindow = infoOverlayWindow;
            AboutWindow = aboutWindow;
        }
    }
}