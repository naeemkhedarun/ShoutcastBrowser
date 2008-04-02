using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShoutcastIntegration.Test
{
    /// <summary>
    /// Summary description for ConfigurationServiceTest
    /// </summary>
    [TestClass]
    public class ConfigurationServiceTest
    {
        [TestMethod]
        public void ConfigurationService_Initialise_IsNotNull()
        {
            IConfigurationService configurationService = CastleWindsorFrameworkHelper.New<IConfigurationService>();
            Assert.AreEqual(1, configurationService.Feeds.Count, "Failed to initialise.");
        }
    }
}
