using System.Collections.Generic;
using System.Collections.ObjectModel;
using ThreadSafeCollections;

namespace ShoutcastIntegration
{
    public interface IStationFeedService
    {
        List<string> GetGenreList();
        SynchronizedObservableCollection<Station> GetStationList();
        SynchronizedObservableCollection<Station> GetStationList(GetBy parameter, string value);
        SynchronizedObservableCollection<Station> CachedStations { get; set; }
        void RefreshLists();
    }
}