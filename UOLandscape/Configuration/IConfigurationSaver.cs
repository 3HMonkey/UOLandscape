namespace UOLandscape.Configuration
{
    internal interface IConfigurationSaver
    {
        void SaveConfiguration<T>(string fileName, T configuration) where T: class;
    }
}