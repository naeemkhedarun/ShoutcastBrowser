using System;
using System.Diagnostics;
using System.Net;

namespace ShoutcastIntegration
{
    public class PlaylistFile
    {
        public PlaylistFile(IConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        private IConfigurationService ConfigurationService { get; set; }

        public void ExecutePlaylist(Station station)
        {
            if (station != null)
            {
                //Download playlist file.
                WebClient client = new WebClient();
                client.DownloadFile(String.Format(ConfigurationService.ShoutcastPlaylistURL, station.ID), "playlist.pls");

                Process.Start("playlist.pls");
            }
        }
    }
}