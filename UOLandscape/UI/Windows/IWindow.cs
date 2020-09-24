namespace UOLandscape.UI.Windows
{
    public interface IWindow
    {
        bool IsVisible { get; }

        void Hide();

        void ToggleVisibility();

        bool Show(uint dockSpaceId);
    }
}