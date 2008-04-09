using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShoutcastIntegration.Test
{
    [TestClass]
    public class WebStreamTest
    {
        private readonly IFeedStream stream = CastleWindsorFrameworkHelper.New<IFeedStream>();

        [TestMethod]
        public void WebStream_Initialise_IsNotNull()
        {
            Assert.IsNotNull(stream, "Failed to initialise.");
        }

        [TestMethod]
        public void GetStream_WithValidURL_IsNotNull()
        {
            List<Stream> streams = stream.GetStream();
            Assert.IsNotNull(streams, "Failed to get stream.");
        }
    }
}