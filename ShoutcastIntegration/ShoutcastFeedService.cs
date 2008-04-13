using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Threading;
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
        private Thread _backgroundThread;
        public volatile bool shutdownThread;

        public ShoutcastFeedService()
        {
            CachedStations = new BeginInvokeOC<Station>(Dispatcher.CurrentDispatcher);
        }

        public IFeedStream FeedStream { get; set; }

        #region IStationFeedService Members

        public BeginInvokeOC<Station> CachedStations { get; set; }

        public List<string> GetGenreList()
        {
            List<String> genres = new List<string>();

            XmlTextReader reader = new XmlTextReader(FeedStream.GetStream(URL));
            while (!reader.EOF)
            {
                if (reader.Name.Equals("genre"))
                {
                    genres.Add(reader["name"]);
                }
                reader.Read();
            }
            return genres;
        }

        public BeginInvokeOC<Station> GetStationList()
        {
            return GetStationList(GetBy.Default, String.Empty);
        }

        public BeginInvokeOC<Station> GetStationList(GetBy parameter, string value)
        {
            if (CachedStations == null)
            {
                CachedStations = new BeginInvokeOC<Station>(Dispatcher.CurrentDispatcher);
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

            if (_backgroundThread != null && _backgroundThread.IsAlive)
            {
                shutdownThread = true;
                while (_backgroundThread.IsAlive)
                {
                    Thread.Sleep(5);
                }
            }

            shutdownThread = false;

            Stream _stream = FeedStream.GetStream(streamURL);
            XmlTextReader reader = new XmlTextReader(_stream);
            CachedStations.Clear();
            _backgroundThread = new Thread(Start) {Priority = ThreadPriority.BelowNormal};
            _backgroundThread.Start(reader);

            return CachedStations;
        }

        public void RefreshLists()
        {
            CachedStations.Clear();
        }

        public void ShutdownService()
        {
            shutdownThread = true;
        }

        #endregion

        private void Start(object obj)
        {
            using (XmlTextReader reader = obj as XmlTextReader)
            {
                try
                {
                    if (reader != null)
                    {
                        while (!reader.EOF && !shutdownThread)
                        {
                            if (reader.Name.Equals("station"))
                            {
                                PopulateStation(reader, CachedStations);
                            }
                            reader.Read();
                        }
                        reader.Close();
                    }
                }
                catch (XmlException)
                {
                    shutdownThread = true;
                }
                catch (WebException)
                {
                    shutdownThread = true;
                }
            }
        }

        private static void PopulateStation(XmlReader reader, ICollection<Station> stations)
        {
            Station station = new Station
                                  {
                                      Name = reader["name"],
                                      ID = Convert.ToInt32(reader["id"]),
                                      Bitrate = Convert.ToInt32(reader["br"]),
                                      CurrentTrack = reader["ct"],
                                      Genre = reader["genre"],
                                      TotalListeners = Convert.ToInt32(reader["tc"]),
                                      Type = reader["mt"]
                                  };
            if (station.ID > 0)
                stations.Add(station);
        }

        #region Nested type: AddStation

        #endregion
    }
}