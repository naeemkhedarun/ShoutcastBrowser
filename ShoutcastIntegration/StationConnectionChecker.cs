using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using PortProberLib;

namespace ShoutcastIntegration
{
    public class StationConnectionChecker
    {
        private readonly INotifyCollectionChanged stationCollection;

        public StationConnectionChecker(IStationFeedService stationFeedService,
                                        IConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
            stationCollection = stationFeedService.CachedStations;
            stationCollection.CollectionChanged += StationCollection_OnCollectionChanged;
        }

        private IConfigurationService ConfigurationService { get; set; }

        private void StationCollection_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
            NotifyCollectionChangedAction action = e.Action;
            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                    CheckConnectionStatus(e.NewItems[0] as Station);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CheckConnectionStatus(Station station)
        {
            if (station != null)
            {
                Thread checkStatusThread = new Thread(Start);
                checkStatusThread.Start(station);
            }
        }

        private void Start(object o)
        {
            try
            {
                Station station = (Station)o;
                WebClient client = new WebClient();
                Stream openRead = client.OpenRead(String.Format(ConfigurationService.ShoutcastPlaylistURL, station.ID));
                StreamReader reader = new StreamReader(openRead);

                bool foundIP = false;
                while (!reader.EndOfStream && !foundIP)
                {
                    string line = reader.ReadLine();
                    Regex regex = new Regex(@"(?<ip>[0-9]+.[0-9]+.[0-9]+.[0-9]+):(?<port>[0-9]+)");
                    Match match = regex.Match(line);

                    if (match.Success)
                    {
                        String IP = match.Groups["ip"].Value;
                        int port = Int32.Parse(match.Groups["port"].Value);


                        IPAddress parsedIPAddress = IPAddress.Parse(IP);
                        if (parsedIPAddress != null)
                        {
                            IPEndPoint endPoint = new IPEndPoint(parsedIPAddress, port);
                            PortProber prober = new PortProber(endPoint);
                            station.IsAlive = prober.ProbeMachine();
                            foundIP = true;
                        }
                    }
                }
            }
            catch (Exception e) { }
        }
    }
}