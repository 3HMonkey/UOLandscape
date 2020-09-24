namespace UOLandscape.UI.Windows
{
    public interface IUIComponent
    {
        bool IsVisible { get; }

        void Hide();

        void ToggleVisibility();

        bool Show(uint dockSpaceId);
    }
}