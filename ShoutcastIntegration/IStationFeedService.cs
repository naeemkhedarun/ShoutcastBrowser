using System.Collections.Generic;

namespace ShoutcastIntegration
{
    public interface IStationFeedService
    {
        List<string> GetGenreList();
        IList<Station> GetStationList();
        IList<Station> GetStationList(string genre);
        void RefreshLists();
    }
}