using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ShoutcastIntegration
{
    public class ShoutcastFeedService : IStationFeedService
    {
        public ShoutcastFeedService(IConfigurationService configurationService, IFeedStream feedStream)
        {
            ConfigurationService = configurationService;
            FeedStream = feedStream;
        }

        public string ShoutcastPlaylistURL { get; set; }

        private IFeedStream FeedStream { get; set; }

        private IConfigurationService ConfigurationService { get; set; }

        #region IStationFeedService Members

        public List<string> GetGenreList()
        {
            throw new NotImplementedException();
        }

        public List<Station> GetStationList()
        {
            foreach (Stream stream in FeedStream.GetStream())
            {
                var reader = new XmlTextReader(stream);
                
            }
        }

        public List<Station> GetStationList(string genre)
        {
            throw new NotImplementedException();
        }

        public void RefreshLists()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}