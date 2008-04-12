using System.IO;

namespace ShoutcastIntegration
{
    public interface IFeedStream
    {
        Stream GetStream(string location);
    }
}