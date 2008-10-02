using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;

namespace ShoutcastIntegration
{
    public class PlaylistFile
    {
        private WebClient client;

        public PlaylistFile(IConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        private IConfigurationService ConfigurationService { get; set; }

        public void ExecutePlaylist(Station station)
        {
            if(client != null) 
                CleanupPreviousPendingRequest();
            else
            {
                client = new WebClient();
                client.DownloadFileCompleted += Client_OnDownloadFileCompleted;                
            }

            if (station != null)
            {
                Debug.WriteLine("Downloading playlist...");
                client.DownloadFileAsync(new Uri(String.Format(ConfigurationService.ShoutcastPlaylistURL, station.ID)), "playlist.pls");                    
            }
        }

        private void CleanupPreviousPendingRequest()
        {
            Debug.WriteLine("Cleaning up previous request...");

            client.CancelAsync();
            client.Dispose();
        }

        private static void Client_OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                Debug.WriteLine("Starting playlist...");
                Process.Start("playlist.pls");       
            }
        }
    }
}