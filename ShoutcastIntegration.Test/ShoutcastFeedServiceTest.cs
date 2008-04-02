using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShoutcastIntegration.Test
{
    /// <summary>
    /// Summary description for ShoutcastFeedServiceTest
    /// </summary>
    [TestClass]
    public class ShoutcastFeedServiceTest
    {
        private readonly IStationFeedService stationFeedService =
            CastleWindsorFrameworkHelper.New<IStationFeedService>();

        [TestMethod]
        public void ShoutcastFeedService_Initialise_IsNotNull()
        {
            Assert.IsNotNull(stationFeedService, "Failed to initialise.");
        }
           
        [TestMethod]
        public void GetStationList_ValidStationFeed_ReturnsCorrectStations()
        {
            List<Station> list = stationFeedService.GetStationList();
            Assert.AreEqual(1000, list.Count, "Failed to get correct number of stations.");
            Assert.AreEqual("Feed One.", list[0].Name, "Incorrect stations.");
        }
    }
}