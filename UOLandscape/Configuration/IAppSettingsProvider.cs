namespace UOLandscape.Configuration
{
    internal interface IAppSettingsProvider
    {
        AppSettings AppSettings { get; }

        void Save();

        void Load();
    }
}