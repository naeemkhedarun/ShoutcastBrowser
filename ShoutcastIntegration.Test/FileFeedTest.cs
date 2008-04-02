using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShoutcastIntegration.Test
{
    /// <summary>
    /// Summary description for FileFeedTest
    /// </summary>
    [TestClass]
    public class FileFeedTest
    {
        private readonly IFeedStream feedStream = CastleWindsorFrameworkHelper.New<IFeedStream>();

        [TestMethod]
        public void FileFeed_Initialise_IsNotNull()
        {
            Assert.IsNotNull(feedStream, "Failed to initialise.");
        }

        [TestMethod]
        public void GetStream_ValidFile_ReturnsStream()
        {
            Assert.IsNotNull(feedStream.GetStream(), "Failed to get stream.");
        }
    }
}