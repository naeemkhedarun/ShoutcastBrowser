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
            CachedStations = new List<Station>();
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
            return GetStationList(String.Empty);
        }

        public IList<Station> GetStationList(string genre)
        {
            if (CachedStations != null && CachedStations.Count > 0)
                return CachedStations;

            if (CachedStations == null) CachedStations = new List<Station>();

            foreach (Stream stream in FeedStream.GetStream())
            {
                var reader = new XmlTextReader(stream);

                while (!reader.EOF)
                {
                    if (reader.Name.Equals("station"))
                    {
                        if (!String.IsNullOrEmpty(genre) && reader["genre"].Equals(genre))
                        {
                            PopulateStation(reader, CachedStations);
                        }else if(String.IsNullOrEmpty(genre))
                        {
                            PopulateStation(reader, CachedStations);
                        }
                    }
                    reader.Read();
                }
            }
            return CachedStations;
        }

        private IList<Station> CachedStations { get; set; }

        private static void PopulateStation(XmlReader reader, ICollection<Station> stations)
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

        public void RefreshLists()
        {
            CachedStations.Clear();
        }

        #endregion
    }
}