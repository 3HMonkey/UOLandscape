namespace UOLandscape.UI.Windows
{
    public interface IUIComponent
    {
        bool IsActive { get; }

        void Hide();

        void ToggleActive();

        bool Show(uint dockSpaceId);
    }
}