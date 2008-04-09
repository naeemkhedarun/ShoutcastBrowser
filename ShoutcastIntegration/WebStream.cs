using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ShoutcastIntegration
{
    public class WebStream : IFeedStream
    {
        public WebStream(IConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        private IConfigurationService ConfigurationService { get; set; }

        #region IFeedStream Members

        public List<Stream> GetStream()
        {
            string playlistURL = ConfigurationService.ShoutcastDirectoryListURL;
            WebClient Client = new WebClient();
            List<Stream> streams = new List<Stream>();
            streams.Add(Client.OpenRead(playlistURL));
            return streams;
        }

        #endregion
    }
}