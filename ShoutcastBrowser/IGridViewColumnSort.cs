using System.ComponentModel;
using System.Windows.Controls;

namespace ShoutcastBrowser
{
    public interface IGridViewColumnSort
    {
        GridViewColumnHeader LastHeaderClicked { get; set; }
        ListSortDirection LastDirection { get; set; }
    }
}