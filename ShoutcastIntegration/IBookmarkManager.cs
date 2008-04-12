using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreadSafeCollections;

namespace ShoutcastIntegration
{
    public interface IBookmarkManager
    {
        SynchronizedObservableCollection<Station> GetBookmarkedStations();
        void BookmarkStation(Station station);
        void RemovedBookmarkedStation(Station station);
    }
}
