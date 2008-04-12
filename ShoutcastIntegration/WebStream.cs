using System.IO;
using System.Net;

namespace ShoutcastIntegration
{
    public class WebStream : IFeedStream
    {
        #region IFeedStream Members

        public Stream GetStream(string location)
        {
            string playlistURL = location;
            WebClient Client = new WebClient();
            return Client.OpenRead(playlistURL);
        }

        #endregion
    }
}