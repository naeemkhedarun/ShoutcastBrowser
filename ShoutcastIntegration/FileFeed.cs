using System.Collections.Generic;
using System.IO;

namespace ShoutcastIntegration
{
    public class FileFeed : IFeedStream
    {
        public IConfigurationService ConfigurationService;

        public FileFeed(IConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        #region IFeedStream Members

        public List<Stream> GetStream()
        {
            var feeds = new List<Stream>();
            foreach (string feed in ConfigurationService.Feeds)
            {
                FileStream open = File.Open(feed, FileMode.Open);
                feeds.Add(open);
            }

            return feeds;
        }

        #endregion
    }
}