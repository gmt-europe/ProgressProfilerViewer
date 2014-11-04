using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ProgressProfilerViewer.Dto;
using SourceGrid;

namespace ProgressProfilerViewer
{
    public partial class FunctionsUserControl : FunctionsBaseUserControl
    {
        private List<Source> _functionSources;

        public FunctionsUserControl()
        {
            InitializeComponent();
        }

        public void LoadFunctions(Session session)
        {
            _grid.FixedRows = 1;
            _grid.ColumnsCount = 6;
            _grid.RowsCount = session.Sources.Count + 1;
            _grid.CustomSort = true;
            _grid.SortingRangeRows += _functions_SortingRangeRows;

            _grid[0, 0] = CreateHeader("Function", false);
            _grid[0, 1] = CreateHeader("Number of Calls", true);
            _grid[0, 2] = CreateHeader("Elapsed Time %", true);
            _grid[0, 3] = CreateHeader("Elapsed Cumulative Time %", true);
            _grid[0, 4] = CreateHeader("Avg Elapsed Time", true);
            _grid[0, 5] = CreateHeader("Avg Elapsed Cumulative Time", true);

            _grid.Columns[0].Width = 400;

            _functionSources = session.Sources.Values.OrderByDescending(p => p.Percentage).ToList();

            _grid.SortColumn(2, false);

            for (int i = 1; i < _grid.Columns.Count; i++)
            {
                _grid.Columns.AutoSizeColumn(i);
            }
        }

        private void ReloadFunctions()
        {
            for (int i = 0; i < _functionSources.Count; i++)
            {
                var source = _functionSources[i];

                _grid[i + 1, 0] = CreateCell(source.DisplayName, false);
                _grid[i + 1, 1] = CreateCell(source.CallCount, true);
                _grid[i + 1, 2] = CreateCell(source.Percentage.ToString("0.00"), true);
                _grid[i + 1, 3] = CreateCell(source.CumulativePercentage.ToString("0.00"), true);
                _grid[i + 1, 4] = CreateCell(source.AverageTime.TotalMilliseconds.ToString("0.00"), true);
                _grid[i + 1, 5] = CreateCell(source.CumulativeTime.TotalMilliseconds.ToString("0.00"), true);
            }

            _grid.Refresh();
        }

        void _functions_SortingRangeRows(object sender, SortRangeRowsEventArgs e)
        {
            Sort(_functionSources, new SortKey(e.KeyColumn, e.Ascending));

            ReloadFunctions();
        }
    }
}
