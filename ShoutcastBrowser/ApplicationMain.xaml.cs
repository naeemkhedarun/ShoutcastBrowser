using System;
using System.ComponentModel;
using System.Net;
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

        private readonly PlaylistFile playlistFile =
            CastleWindsorFrameworkHelper.New<PlaylistFile>();

        private readonly IBookmarkManager bookmarkManager;
        private readonly IStationFeedService stationFeedService;

        public ApplicationMain()
        {
            InitializeComponent();
            stationFeedService = CastleWindsorFrameworkHelper.New<IStationFeedService>();
            StationConnectionChecker stationConnectionChecker = new StationConnectionChecker(stationFeedService, CastleWindsorFrameworkHelper.New<IConfigurationService>());
            bookmarkManager = CastleWindsorFrameworkHelper.New<IBookmarkManager>();

            bookmarksListView.ItemsSource =
                new ReadOnlySynchronizedObservableCollection<Station>(bookmarkManager.GetBookmarkedStations());

            try
            {
                genreComboBox.ItemsSource = stationFeedService.GetGenreList();
            }
            catch (Exception)
            {
                OfflineMessageBox();
            }
        }

        #region IGridViewColumnSort Members

        public GridViewColumnHeader LastHeaderClicked { get; set; }
        public ListSortDirection LastDirection { get; set; }

        #endregion

        private static void OfflineMessageBox()
        {
            MessageBox.Show("Unable to establish connection.", "Unable to connect.", MessageBoxButton.OK);
        }

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

        private void GetStations(GetBy getBy, String value, ItemsControl listView)
        {
            try
            {
                BeginInvokeOC<Station> list = stationFeedService.GetStationList(getBy, value);
                if (listView.ItemsSource == null)
                {
                    listView.ItemsSource = list;
                }
            }
            catch (WebException)
            {
                OfflineMessageBox();
            }
        }

        private void searchTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                String searchText = searchTextBox.Text;
                if (String.IsNullOrEmpty(searchText))
                {
                    GetStations(GetBy.Default, String.Empty, stationsListView);
                }
                else
                {
                    GetStations(GetBy.Search, searchText, stationsListView);
                }
            }
        }


        private void genreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string genre = genreComboBox.SelectedValue as string;
            GetStations(GetBy.Genre, genre, stationsListView);
        }

        private void clearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = "Search...";
        }

        private void playMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Station station = stationsListView.SelectedItem as Station;

            playlistFile.ExecutePlaylist(station);
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

            playlistFile.ExecutePlaylist(station);
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