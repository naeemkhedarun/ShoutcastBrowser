using System;
using System.Diagnostics;
using System.Net;

namespace ShoutcastIntegration
{
    public class ExecutePlaylistFile
    {
        public ExecutePlaylistFile(IConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        private IConfigurationService ConfigurationService { get; set; }

        public void ExecutePlaylist(Station station)
        {
            //Download playlist file.
            var client = new WebClient();
            client.DownloadFile(String.Format(ConfigurationService.ShoutcastPlaylistURL, station.ID), "playlist.pls");

            Process.Start("playlist.pls");
        }
    }
}