using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ShoutcastIntegration
{
    public class ShoutcastFeedService : IStationFeedService
    {
        public ShoutcastFeedService(IConfigurationService configurationService, IFeedStream feedStream)
        {
            ConfigurationService = configurationService;
            FeedStream = feedStream;
        }

        public string ShoutcastPlaylistURL { get; set; }

        private IFeedStream FeedStream { get; set; }

        private IConfigurationService ConfigurationService { get; set; }

        #region IStationFeedService Members

        public List<string> GetGenreList()
        {
            throw new NotImplementedException();
        }

        public IList<Station> GetStationList()
        {
            IList<Station> stations = new List<Station>();
            
            foreach (Stream stream in FeedStream.GetStream())
            {
                var reader = new XmlTextReader(stream);

                while (!reader.EOF)
                {
                    if (reader.Name.Equals("station"))
                    {
                        Station station = new Station();
                        station.Name = reader["name"];
                        station.ID = Convert.ToInt32(reader["id"]);
                        station.Bitrate = reader["br"];
                        station.CurrentTrack = reader["ct"];
                        station.Genre = reader["genre"];
                        station.TotalListeners = Convert.ToInt32(reader["tc"]);
                        station.Type = reader["mt"];
                        stations.Add(station);
                    }
                    reader.Read();
                }
            }
            return stations;
        }

        public List<Station> GetStationList(string genre)
        {
            throw new NotImplementedException();
        }

        public void RefreshLists()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}