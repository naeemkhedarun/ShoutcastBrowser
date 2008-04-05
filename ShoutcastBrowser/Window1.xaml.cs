using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ShoutcastIntegration;

namespace ShoutcastBrowser
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            IStationFeedService stationFeedService = CastleWindsorFrameworkHelper.New<IStationFeedService>();
            stationsListView.ItemsSource = stationFeedService.GetStationList();
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (_lastHeaderClicked != null)
                {
                    _lastHeaderClicked.Column.HeaderTemplate = null;
                }

                if (headerClicked != _lastHeaderClicked)
                {
                    direction = ListSortDirection.Ascending;
                }
                else
                {
                    if (_lastDirection == ListSortDirection.Ascending)
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                    }
                }

                string header = headerClicked.Column.Header as string;
                Sort(header, direction);

//                if (direction == ListSortDirection.Ascending)
//                {
////                    headerClicked.Column.HeaderTemplate =
////                      Resources["HeaderTemplateArrowUp"] as DataTemplate;
//                }
//                else
//                {
//                    headerClicked.Column.HeaderTemplate =
//                      Resources["HeaderTemplateArrowDown"] as DataTemplate;
//                }

                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }
        }


        private void Sort(string sortBy, ListSortDirection direction)
        {
            stationsListView.Items.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            stationsListView.Items.SortDescriptions.Add(sd);
            stationsListView.Items.Refresh();
        }
    }
}
