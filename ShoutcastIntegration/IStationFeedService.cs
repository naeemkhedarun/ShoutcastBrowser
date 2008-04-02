using System.Collections.Generic;

namespace ShoutcastIntegration
{
    public interface IStationFeedService
    {
        List<string> GetGenreList();
        List<Station> GetStationList();
        List<Station> GetStationList(string genre);
        void RefreshLists();
    }
}