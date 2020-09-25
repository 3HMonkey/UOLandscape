namespace UOLandscape.UI.Windows
{
    internal abstract class Window : IWindow
    {
        protected bool _isVisible;

        public bool IsVisible => _isVisible;

        public void Hide()
        {
            _isVisible = false;
        }

        public void ToggleVisibility()
        {
            _isVisible = !_isVisible;
        }

        public abstract bool Show(uint dockSpaceId);
    }
}