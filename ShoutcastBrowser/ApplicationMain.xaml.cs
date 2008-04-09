using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ShoutcastIntegration;

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

        public ApplicationMain()
        {
            InitializeComponent();
            var stationFeedService = CastleWindsorFrameworkHelper.New<IStationFeedService>();
            stationsListView.ItemsSource = stationFeedService.GetStationList();
        }

        #region IGridViewColumnSort Members

        public GridViewColumnHeader LastHeaderClicked { get; set; }
        public ListSortDirection LastDirection { get; set; }

        #endregion

        [STAThread]
        private static void Main(string[] args)
        {
            var app = new Application();
            var prog = CastleWindsorFrameworkHelper.New<ApplicationMain>();
            app.MainWindow = prog;
            app.ShutdownMode = ShutdownMode.OnMainWindowClose;
            app.Run(prog);
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnSorter.OnClictSorting(stationsListView, e, this);
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            var station = stationsListView.SelectedItem as Station;

            _executePlaylistFile.ExecutePlaylist(station);
        }

        private void searchTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
//                searchTextBox
            }
        }
    }
}