using System.IO;

namespace ShoutcastIntegration
{
    public class FileFeed : IFeedStream
    {
        #region IFeedStream Members

        public Stream GetStream(string location)
        {
            return File.Open(location, FileMode.OpenOrCreate);
        }

        #endregion
    }
}