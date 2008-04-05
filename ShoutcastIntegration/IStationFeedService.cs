using System.Collections.Generic;

namespace ShoutcastIntegration
{
    public interface IStationFeedService
    {
        List<string> GetGenreList();
        IList<Station> GetStationList();
        List<Station> GetStationList(string genre);
        void RefreshLists();
    }
}