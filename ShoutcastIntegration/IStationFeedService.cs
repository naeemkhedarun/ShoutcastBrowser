using System.Collections.Generic;
using ThreadSafeCollections;

namespace ShoutcastIntegration
{
    public interface IStationFeedService
    {
        BeginInvokeOC<Station> CachedStations { get; set; }
        List<string> GetGenreList();
        BeginInvokeOC<Station> GetStationList();
        BeginInvokeOC<Station> GetStationList(GetBy parameter, string value);
        void RefreshLists();
        void ShutdownService();
    }
}