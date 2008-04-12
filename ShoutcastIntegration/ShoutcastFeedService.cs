using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using ThreadSafeCollections;

namespace ShoutcastIntegration
{
    public class ShoutcastFeedService : IStationFeedService
    {
        private const string DEFAULT_CMD = @"random=20";
        private const string GENRE_CMD = @"genre={0}";
        private const string SEARCH_CMD = @"search={0}";
        private const string URL = @"http://www.shoutcast.com/sbin/newxml.phtml?";

        public ShoutcastFeedService()
        {
            CachedStations = new SynchronizedObservableCollection<Station>();
        }

        public IFeedStream FeedStream { get; set; }

        #region IStationFeedService Members

        public SynchronizedObservableCollection<Station> CachedStations { get; set; }

        public List<string> GetGenreList()
        {
            List<String> genres = new List<string>();

            XmlTextReader reader = new XmlTextReader(FeedStream.GetStream(URL));
            while (!reader.EOF)
            {
                if (reader.Name.Equals("genre"))
                {
                    if (genres.Count < 51) genres.Add(reader["name"]);
                }
                reader.Read();
            }
            return genres;
        }

        public SynchronizedObservableCollection<Station> GetStationList()
        {
            return GetStationList(GetBy.Default, String.Empty);
        }

        public SynchronizedObservableCollection<Station> GetStationList(GetBy parameter, string value)
        {
            if (CachedStations == null)
            {
                CachedStations = new SynchronizedObservableCollection<Station>();
            }
            else
            {
                CachedStations.Clear();
            }

            String streamURL;

            switch (parameter)
            {
                case GetBy.Search:
                    streamURL = String.Format(URL + SEARCH_CMD, value);
                    break;
                case GetBy.Genre:
                    streamURL = String.Format(URL + GENRE_CMD, value);
                    break;
                case GetBy.Default:
                    streamURL = String.Format(URL + DEFAULT_CMD);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("parameter");
            }
            XmlTextReader reader = new XmlTextReader(FeedStream.GetStream(streamURL));

            Thread backgroundThread = new Thread(Start);
            backgroundThread.Start(reader);


            return CachedStations;
        }

        public void RefreshLists()
        {
            CachedStations.Clear();
        }

        #endregion

        private void Start(object obj)
        {
            XmlTextReader reader = obj as XmlTextReader;

            try
            {
                if (reader != null)
                    while (!reader.EOF)
                    {
                        if (reader.Name.Equals("station"))
                        {
                            PopulateStation(reader, CachedStations);
                            Console.Out.WriteLine("Found station.");
                        }
                        reader.Read();
                    }
            }
            catch (XmlException)
            {
                //Invalid xml, no results.
                CachedStations = new SynchronizedObservableCollection<Station>();
            }
        }

        private static void PopulateStation(XmlReader reader, ICollection<Station> stations)
        {
            Station station = new Station
                                  {
                                      Name = reader["name"],
                                      ID = Convert.ToInt32(reader["id"]),
                                      Bitrate = reader["br"],
                                      CurrentTrack = reader["ct"],
                                      Genre = reader["genre"],
                                      TotalListeners = Convert.ToInt32(reader["tc"]),
                                      Type = reader["mt"]
                                  };
            if (station.ID > 0)
                stations.Add(station);
        }

        #region Nested type: AddStation

        private delegate void AddStation(Station station);

        #endregion
    }
}