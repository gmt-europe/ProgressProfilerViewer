using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceGrid;
using SourceGrid.Cells.Controllers;
using SourceGrid.Cells.Virtual;

namespace ProgressProfilerViewer
{
    public static class GridExtensions
    {
        public static void SortColumn(this Grid grid, int column, bool ascending)
        {
            var sortHeaderModel = (SourceGrid.Cells.Models.SortableHeader)((CellVirtual)grid[0, 3]).Model.FindModel(typeof(SourceGrid.Cells.Models.SortableHeader));
            var sortHeaderController = ((CellVirtual)grid[0, 3]).FindController<SortableHeader>();
            var cellContext = new CellContext(grid, new Position(0, 3));
            var status = sortHeaderModel.GetSortStatus(cellContext);
            sortHeaderController.SortColumn(cellContext, ascending, status.Comparer);
        }
    }
}
