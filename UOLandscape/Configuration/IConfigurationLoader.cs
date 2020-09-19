namespace UOLandscape.Configuration
{
    internal interface IConfigurationLoader
    {
        T LoadConfiguration<T>(string fileName) where T : class;
    }
}