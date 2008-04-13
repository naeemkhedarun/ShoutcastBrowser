using System;
using System.IO;
using System.Xml;
using ThreadSafeCollections;

namespace ShoutcastIntegration
{
    public class BookmarkManager : IBookmarkManager
    {
        private SynchronizedObservableCollection<Station> _bookmarks;

        public BookmarkManager()
        {
            _bookmarks = new SynchronizedObservableCollection<Station>();
        }


        public IFeedStream FeedStream { get; set; }

        #region IBookmarkManager Members

        public SynchronizedObservableCollection<Station> GetBookmarkedStations()
        {
            XmlTextReader reader = new XmlTextReader(FeedStream.GetStream("Bookmarks/bookmarks.xml"));
            while (!reader.EOF)
            {
                if (reader.Name.Equals("bookmark"))
                {
                    AddBookmark(reader);
                }
                reader.Read();
            }

            return _bookmarks;
        }

        public void BookmarkStation(Station station)
        {
            _bookmarks.Add(station);
            XmlDocument document = new XmlDocument();
            Stream stream = FeedStream.GetStream("Bookmarks/bookmarks.xml");
            document.Load(stream);
            stream.Close();

            XmlNode bookmarksNode = document.SelectSingleNode("bookmarks");
            XmlNode newBookmarkNode = document.CreateNode(XmlNodeType.Element, "bookmark", null);

            newBookmarkNode.Attributes.Append(CreateAttribute(document, "name", station.Name));
            newBookmarkNode.Attributes.Append(CreateAttribute(document, "id", station.ID.ToString()));
            newBookmarkNode.Attributes.Append(CreateAttribute(document, "br", station.Bitrate.ToString()));
            newBookmarkNode.Attributes.Append(CreateAttribute(document, "ct", station.CurrentTrack));
            newBookmarkNode.Attributes.Append(CreateAttribute(document, "genre", station.Genre));
            newBookmarkNode.Attributes.Append(CreateAttribute(document, "tc", station.TotalListeners.ToString()));
            newBookmarkNode.Attributes.Append(CreateAttribute(document, "mt", station.Type));

            bookmarksNode.AppendChild(newBookmarkNode);
            document.Save("Bookmarks/bookmarks.xml");
        }

        public void RemovedBookmarkedStation(Station station)
        {
            _bookmarks.Remove(station);
            Stream stream = FeedStream.GetStream("Bookmarks/bookmarks.xml");
            XmlDocument document = new XmlDocument();
            document.Load(stream);
            stream.Close();

            XmlNode node = document.SelectSingleNode("bookmarks");
            foreach (XmlElement element in node)
            {
                if (element.Attributes["id"] != null && element.Attributes["id"].Value.Equals(station.ID.ToString()))
                {
                    node.RemoveChild(element);
                }
            }
            document.Save("Bookmarks/bookmarks.xml");
        }

        #endregion

        private static XmlAttribute CreateAttribute(XmlDocument document, String name, String value)
        {
            XmlAttribute attribute = document.CreateAttribute(name);
            attribute.Value = value;
            return attribute;
        }

        private void AddBookmark(XmlReader reader)
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

            _bookmarks.Add(station);
        }
    }
}