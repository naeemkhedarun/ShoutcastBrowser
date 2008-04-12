using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ShoutcastIntegration;
using ThreadSafeCollections;

namespace ShoutcastBrowser
{
    public interface IGridViewColumnSort
    {
        GridViewColumnHeader LastHeaderClicked { get; set; }
        ListSortDirection LastDirection { get; set; }
    }

    /// <summary>
    /// Interaction logic for ApplicationMain.xaml
    /// </summary>
    public partial class ApplicationMain : IGridViewColumnSort
    {
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
            bookmarksListView.ItemsSource = new BindableCollection<Station>(bookmarkManager.GetBookmarkedStations());
        }

        #region IGridViewColumnSort Members

        public GridViewColumnHeader LastHeaderClicked { get; set; }
        public ListSortDirection LastDirection { get; set; }

        #endregion

        [STAThread]
        private static void Main()
        {
            Application app = new Application();
            ApplicationMain prog = CastleWindsorFrameworkHelper.New<ApplicationMain>();
            app.MainWindow = prog;
            app.ShutdownMode = ShutdownMode.OnMainWindowClose;
            app.Run(prog);
        }

        private void StationsGridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnSorter.OnClictSorting(stationsListView, e, this);
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            Station station = stationsListView.SelectedItem as Station;

            _executePlaylistFile.ExecutePlaylist(station);
        }

        private void searchTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                String searchText = searchTextBox.Text;
                stationsListView.ItemsSource = String.IsNullOrEmpty(searchText) 
                    ? new BindableCollection<Station>(stationFeedService.GetStationList(GetBy.Default, String.Empty)) 
                    : new BindableCollection<Station>(stationFeedService.GetStationList(GetBy.Search, searchText));
            }
        }

        private void genreComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string genre = genreComboBox.SelectedValue as string;
            stationsListView.ItemsSource =
                new BindableCollection<Station>(stationFeedService.GetStationList(GetBy.Genre, genre));
        }

        private void clearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = String.Empty;
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
    }
}