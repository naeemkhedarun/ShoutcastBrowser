using System.Collections.Generic;

namespace ShoutcastIntegration
{
    public interface IConfigurationService
    {
        List<string> Feeds { get; set; }
        string ShoutcastPlaylistURL { get; set; }
        string ShoutcastDirectoryListURL { get; set; }
    }
}