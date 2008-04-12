using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ShoutcastBrowser
{
    public class GridViewColumnSorter
    {
        public static void OnClictSorting(ItemsControl listView, RoutedEventArgs e, IGridViewColumnSort columnSort)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (headerClicked != null)
            {
                if (columnSort.LastHeaderClicked != null)
                {
                    columnSort.LastHeaderClicked.Column.HeaderTemplate = null;
                }

                ListSortDirection direction;
                if (headerClicked != columnSort.LastHeaderClicked)
                {
                    direction = ListSortDirection.Ascending;
                }
                else
                {
                    direction = columnSort.LastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }

                if (headerClicked.Column != null)
                {
                    var header = headerClicked.Column.Header as string;
                    Sort(listView, header, direction);

                    columnSort.LastHeaderClicked = headerClicked;
                    columnSort.LastDirection = direction;
                }
            }
        }

        public static void Sort(ItemsControl listView, string sortBy, ListSortDirection direction)
        {
            listView.Items.SortDescriptions.Clear();
            var sd = new SortDescription(sortBy, direction);
            listView.Items.SortDescriptions.Add(sd);
            listView.Items.Refresh();
        }
    }
}