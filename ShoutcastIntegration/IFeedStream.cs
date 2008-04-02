using System.Collections.Generic;
using System.IO;

namespace ShoutcastIntegration
{
    public interface IFeedStream
    {
        List<Stream> GetStream();
    }
}