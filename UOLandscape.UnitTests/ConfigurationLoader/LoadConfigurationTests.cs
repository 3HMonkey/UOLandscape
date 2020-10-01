using System;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Serilog;
using Xunit;

namespace UOLandscape.UnitTests.ConfigurationLoader
{
    public class LoadConfigurationTests
    {
        [Fact]
        public void Loading_Configuration_From_NotExistingPath_Will_Return_Null()
        {
            var logger = Substitute.For<ILogger>();
            var configurationLoader = new Configuration.ConfigurationLoader(logger);

            configurationLoader
                .LoadConfiguration<TestConfig.Test>("FileDoesNotExist")
                .Should().BeNull();
        }

        [Fact]
        public void Loading_Configuration_From_ExistingPath_Will_Return_Null()
        {
            var logger = Substitute.For<ILogger>();
            var configurationLoader = new Configuration.ConfigurationLoader(logger);

            using var testConfig = new TestConfig();
            configurationLoader
                .LoadConfiguration<TestConfig.Test>(testConfig.TempFileName)
                .Should().NotBeNull();
        }

        private class TestConfig : IDisposable
        {
            public class Test
            {
            }

            public string TempFileName { get; }

            public TestConfig()
            {
                TempFileName = Path.GetTempFileName();
                var json = JsonConvert.SerializeObject(new Test());
                File.WriteAllText(TempFileName, json);
            }

            public void Dispose()
            {
                File.Delete(TempFileName);
            }
        }
    }
}