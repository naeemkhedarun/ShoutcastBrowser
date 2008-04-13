using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ShoutcastIntegration;
using ThreadSafeCollections;

namespace ShoutcastBrowser
{
    /// <summary>
    /// Interaction logic for ApplicationMain.xaml
    /// </summary>
    public partial class ApplicationMain : IGridViewColumnSort
    {
        private static readonly Application _app = new Application();

        private readonly ExecutePlaylistFile _executePlaylistFile =
            CastleWindsorFrameworkHelper.New<ExecutePlaylistFile>();

        private readonly IBookmarkManager bookmarkManager;
        private readonly IStationFeedService stationFeedService;

        public ApplicationMain()
        {
            InitializeComponent();
            stationFeedService = CastleWindsorFrameworkHelper.New<IStationFeedService>();
            bookmarkManager = CastleWindsorFrameworkHelper.New<IBookmarkManager>();
            genreComboBox.ItemsSource = stationFeedService.GetGenreList();
            bookmarksListView.ItemsSource =
                new ReadOnlySynchronizedObservableCollection<Station>(bookmarkManager.GetBookmarkedStations());
        }

        #region IGridViewColumnSort Members

        public GridViewColumnHeader LastHeaderClicked { get; set; }
        public ListSortDirection LastDirection { get; set; }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            stationFeedService.ShutdownService();
            _app.Shutdown(0);
        }

        [STAThread]
        private static void Main()
        {
            ApplicationMain prog = CastleWindsorFrameworkHelper.New<ApplicationMain>();
            _app.MainWindow = prog;
            _app.ShutdownMode = ShutdownMode.OnLastWindowClose;
            _app.Run(prog);
        }

        private void StationsGridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnSorter.OnClictSorting(stationsListView, e, this);
        }

        private void searchTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                String searchText = searchTextBox.Text;
                if (String.IsNullOrEmpty(searchText))
                {
                    if (stationsListView.ItemsSource == null)
                    {
                        stationsListView.ItemsSource = stationFeedService.GetStationList(GetBy.Default, String.Empty);
                    }
                    else
                    {
                        stationFeedService.GetStationList(GetBy.Default, String.Empty);
                    }
                }
                else
                {
                    if (stationsListView.ItemsSource == null)
                    {
                        stationsListView.ItemsSource = stationFeedService.GetStationList(GetBy.Search, searchText);
                    }
                    else
                    {
                        stationFeedService.GetStationList(GetBy.Search, searchText);
                    }
                }
            }
        }


        private void genreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string genre = genreComboBox.SelectedValue as string;
            if (stationsListView.ItemsSource == null)
            {
                stationsListView.ItemsSource = stationFeedService.GetStationList(GetBy.Genre, genre);
            }
            else
            {
                stationFeedService.GetStationList(GetBy.Genre, genre);
            }
        }

        private void clearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = "Search...";
        }

        private void playMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Station station = stationsListView.SelectedItem as Station;

            _executePlaylistFile.ExecutePlaylist(station);
        }

        private void bookmarkMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Station station = stationsListView.SelectedItem as Station;
            bookmarkManager.BookmarkStation(station);
        }

        private void BookmarksGridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnSorter.OnClictSorting(bookmarksListView, e, this);
        }

        private void PlayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Station station = bookmarksListView.SelectedItem as Station;

            _executePlaylistFile.ExecutePlaylist(station);
        }

        private void RemoveMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Station station = bookmarksListView.SelectedItem as Station;
            bookmarkManager.RemovedBookmarkedStation(station);
        }

        private void searchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text.Equals("Search..."))
                searchTextBox.Text = String.Empty;
        }

        private void SearchTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == String.Empty)
                searchTextBox.Text = "Search...";
        }
    }
}