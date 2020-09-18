namespace UOLandscape.UI
{
    public interface IUIComponent
    {
        bool IsActive { get; }

        void Hide();

        void ToggleActive();

        bool Show(uint dockSpaceId);
    }
}