using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevAge.Drawing;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Models;
using ColumnHeader = SourceGrid.Cells.ColumnHeader;
using SortableHeader = SourceGrid.Cells.Controllers.SortableHeader;
using TextRenderer = DevAge.Drawing.VisualElements.TextRenderer;

namespace ProgressProfilerViewer
{
    public class FunctionsBaseUserControl : UserControl
    {
        private static readonly SourceGrid.Cells.Views.ColumnHeader ColumnHeaderView = new SourceGrid.Cells.Views.ColumnHeader
        {
            ElementText = new TextRenderer()
        };

        private static readonly SourceGrid.Cells.Views.Cell LeftAlignView = new SourceGrid.Cells.Views.Cell
        {
            ElementText = new TextRenderer()
        };

        private static readonly SourceGrid.Cells.Views.Cell RightAlignView = new SourceGrid.Cells.Views.Cell
        {
            TextAlignment = DevAge.Drawing.ContentAlignment.MiddleRight,
            ElementText = new TextRenderer()
        };

        protected void Sort<T>(List<T> sources, SortKey sortKey)
            where T : ISource
        {
            switch (sortKey.Column)
            {
                case 0:
                    sources.Sort((a, b) => AdjustCompare(String.Compare(a.DisplayName, b.DisplayName, StringComparison.CurrentCultureIgnoreCase), sortKey.Ascending));
                    break;

                case 1:
                    sources.Sort((a, b) => AdjustCompare(a.CallCount.CompareTo(b.CallCount), sortKey.Ascending));
                    break;

                case 2:
                    sources.Sort((a, b) => AdjustCompare(a.Percentage.CompareTo(b.Percentage), sortKey.Ascending));
                    break;

                case 3:
                    sources.Sort((a, b) => AdjustCompare(a.CumulativePercentage.CompareTo(b.CumulativePercentage), sortKey.Ascending));
                    break;

                case 4:
                    sources.Sort((a, b) => AdjustCompare(a.AverageTime.CompareTo(b.AverageTime), sortKey.Ascending));
                    break;

                case 5:
                    sources.Sort((a, b) => AdjustCompare(a.CumulativeTime.CompareTo(b.CumulativeTime), sortKey.Ascending));
                    break;
            }
        }

        private int AdjustCompare(int compare, bool ascending)
        {
            if (!ascending)
                compare = -compare;

            return compare;
        }

        protected ICell CreateHeader(string value, bool initialDescending)
        {
            var header = new ColumnHeader(value)
            {
                View = ColumnHeaderView
            };

            if (initialDescending)
            {
                header.RemoveController(header.FindController<SortableHeader>());
                header.AddController(InitialDescendingSortableHeader.Instance);
            }

            return header;
        }

        private class InitialDescendingSortableHeader : SortableHeader
        {
            public static readonly InitialDescendingSortableHeader Instance = new InitialDescendingSortableHeader();

            private InitialDescendingSortableHeader()
            {
            }

            public override void OnMouseUp(CellContext sender, MouseEventArgs e)
            {
                //Note: I can't use the click event because I don't have Button information (Control.MouseButtons returns always None inside the Click event)

                var currentPoint = sender.Grid.PointToClient(MousePosition);
                var cellRect = sender.Grid.PositionToRectangle(sender.Position);
                float distance;
                var partType = LogicalBorder.GetPointPartType(cellRect, currentPoint, out distance);

                //eseguo il sort solo se non sono attualmente in resize
                if (IsSortEnable(sender) &&
                    partType == RectanglePartType.ContentArea &&
                    e.Button == MouseButtons.Left)
                {
                    ISortableHeader sortHeader = (ISortableHeader)sender.Cell.Model.FindModel(typeof(ISortableHeader));
                    SortStatus l_Status = sortHeader.GetSortStatus(sender);
                    if (l_Status.Style == HeaderSortStyle.Descending)
                        SortColumn(sender, true, l_Status.Comparer);
                    else
                        SortColumn(sender, false, l_Status.Comparer);
                }
            }
        }

        protected ICell CreateCell(object value, bool rightAlign)
        {
            return new Cell(value)
            {
                View = rightAlign ? RightAlignView : LeftAlignView
            };
        }
    }
}
