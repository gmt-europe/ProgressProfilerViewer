using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevAge.Drawing;
using DevAge.Drawing.VisualElements;
using ProgressProfilerViewer.Dto;
using ProgressProfilerViewer.Properties;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Cells.Controllers;

namespace ProgressProfilerViewer
{
    public partial class CallTreeUserControl : FunctionsBaseUserControl
    {
        private Line _rootLine;
        private SortKey _sortKey;
        private Session _session;

        public CallTreeUserControl()
        {
            InitializeComponent();
        }

        public void LoadFunctions(Session session)
        {
            _session = session;

            _grid.FixedRows = 1;
            _grid.ColumnsCount = 6;
            _grid.RowsCount = 1;
            _grid.CustomSort = true;
            _grid.SortingRangeRows += _functions_SortingRangeRows;

            _grid[0, 0] = CreateHeader("Function", false);
            _grid[0, 1] = CreateHeader("Number of Calls", true);
            _grid[0, 2] = CreateHeader("Elapsed Time %", true);
            _grid[0, 3] = CreateHeader("Elapsed Cumulative Time %", true);
            _grid[0, 4] = CreateHeader("Avg Elapsed Time", true);
            _grid[0, 5] = CreateHeader("Avg Elapsed Cumulative Time", true);

            _grid.Columns[0].Width = 400;

            _rootLine = new Line(session.RootSource, 0);

            ReloadFunctions();

            _grid.SortColumn(2, false);

            for (int i = 1; i < _grid.Columns.Count; i++)
            {
                _grid.Columns.AutoSizeColumn(i);
            }
        }

        private void ReloadFunctions()
        {
            _grid.RowsCount = 2;

            int row = 1;

            AddLine(_rootLine, ref row);

            _grid.Refresh();
        }

        private void AddLine(Line line, ref int row)
        {
            _grid[row, 0] = new FunctionCell(this, line);
            _grid[row, 1] = CreateCell(line.CallCount, true);
            _grid[row, 2] = CreateCell(line.Percentage.ToString("0.00"), true);
            _grid[row, 3] = CreateCell(line.CumulativePercentage.ToString("0.00"), true);
            _grid[row, 4] = CreateCell(line.AverageTime.TotalMilliseconds.ToString("0.00"), true);
            _grid[row, 5] = CreateCell(line.CumulativeTime.TotalMilliseconds.ToString("0.00"), true);

            if (line.IsExpanded)
            {
                _grid.Rows.InsertRange(row + 1, line.Lines.Count);

                foreach (var subLine in line.Lines)
                {
                    row++;

                    AddLine(subLine, ref row);
                }
            }
        }

        void _functions_SortingRangeRows(object sender, SortRangeRowsEventArgs e)
        {
            SortLine(_rootLine, new SortKey(e.KeyColumn, e.Ascending));

            ReloadFunctions();
        }

        private void SortLine(Line line, SortKey sortKey)
        {
            _sortKey = sortKey;

            if (line.Lines.Count > 0)
            {
                Sort(line.Lines, sortKey);

                foreach (var subLine in line.Lines)
                {
                    SortLine(subLine, sortKey);
                }
            }
        }

        private class Line : ISource
        {
            public int Level { get; private set; }
            public List<Line> Lines { get; private set; }
            public bool IsExpanded { get; set; }

            public bool HasChildren
            {
                get { return CallTargets.Count > 0; }
            }

            public Line(Source source, int level)
            {
                DisplayName = source.DisplayName;
                CallCount = source.CallCount;
                Percentage = source.Percentage;
                CumulativePercentage = source.CumulativePercentage;
                TotalTime = source.TotalTime;
                TotalCumulativeTime = source.TotalCumulativeTime;
                AverageTime = source.AverageTime;
                CumulativeTime = source.CumulativeTime;
                CallTargets = new List<Call>(source.CallTargets);
                Level = level;

                Lines = new List<Line>();
            }

            public TimeSpan TotalCumulativeTime { get; set; }
            public TimeSpan TotalTime { get; set; }
            public string DisplayName { get; set; }
            public int CallCount { get; set; }
            public double Percentage { get; set; }
            public double CumulativePercentage { get; set; }
            public TimeSpan AverageTime { get; set; }
            public TimeSpan CumulativeTime { get; set; }
            public List<Call> CallTargets { get; set; }
        }

        private class FunctionCell : Cell
        {
            public FunctionCell(CallTreeUserControl parent, Line cellValue)
                : base(cellValue)
            {
                View = new FunctionView(cellValue);
                AddController(new FunctionController(parent, cellValue));
            }

            private class FunctionController : ControllerBase
            {
                private readonly CallTreeUserControl _parent;
                private readonly Line _line;

                public FunctionController(CallTreeUserControl parent, Line line)
                {
                    _line = line;
                    _parent = parent;
                }

                public override void OnDoubleClick(CellContext sender, EventArgs e)
                {
                    if (!_line.HasChildren)
                        return;

                    _line.IsExpanded = !_line.IsExpanded;

                    int row = sender.Position.Row;

                    if (_line.IsExpanded)
                    {
                        if (_line.Lines.Count == 0)
                        {
                            var lines = new Dictionary<Source, Line>();

                            foreach (var call in _line.CallTargets)
                            {
                                var source = call.Callee;
                                Line line;
                                if (lines.TryGetValue(source, out line))
                                {
                                    line.CallCount += call.Count;
                                }
                                else
                                {
                                    line = new Line(call.Callee, _line.Level + 1);
                                    line.CallCount = call.Count;
                                    lines.Add(call.Callee, line);
                                    _line.Lines.Add(line);
                                }
                            }

                            foreach (var line in _line.Lines)
                            {
                                line.AverageTime = new TimeSpan(line.TotalTime.Ticks / line.CallCount);
                                line.CumulativeTime = new TimeSpan(line.TotalCumulativeTime.Ticks / line.CallCount);
                                line.Percentage = line.TotalTime.TotalSeconds * 100 / _parent._session.TotalTime.TotalSeconds;
                                line.CumulativePercentage = line.TotalCumulativeTime.TotalSeconds * 100 / _parent._session.TotalTime.TotalSeconds;
                            }

                            if (_parent._sortKey != null)
                                _parent.Sort(_line.Lines, _parent._sortKey);
                        }

                        _parent._grid.Rows.InsertRange(row + 1, _line.Lines.Count);

                        foreach (var line in _line.Lines)
                        {
                            row++;

                            _parent.AddLine(line, ref row);
                        }
                    }
                    else
                    {
                        _parent._grid.Rows.RemoveRange(row + 1, CalculateVisible(_line.Lines));
                    }
                }

                private int CalculateVisible(List<Line> lines)
                {
                    int result = 0;

                    foreach (var line in lines)
                    {
                        result++;

                        if (line.IsExpanded)
                            result += CalculateVisible(line.Lines);
                    }

                    return result;
                }
            }

            private class FunctionView : SourceGrid.Cells.Views.Cell
            {
                public FunctionView(Line line)
                {
                    ElementText = new FunctionText(line);
                }

                private class FunctionText : Text
                {
                    private readonly Line _line;
                    private static readonly Bitmap TickClosed = Resources.TickClosed;
                    private static readonly Bitmap TickOpen = Resources.TickOpen;
                    private static readonly int TickSize = TickClosed.Width;
                    private const int LevelIndent = 14;

                    private readonly TextFormatFlags _textFormatFlags;

                    public FunctionText(Line line)
                    {
                        _line = line;
                        _textFormatFlags = TextFormatFlags.Default | TextFormatFlags.NoPrefix;
                    }

                    private FunctionText(FunctionText other)
                        : base(other)
                    {
                        _line = other._line;
                        _textFormatFlags = other._textFormatFlags;
                    }

                    public override object Clone()
                    {
                        return new FunctionText(this);
                    }

                    public override string Value
                    {
                        get { return _line.DisplayName; }
                        set { }
                    }

                    protected override void OnDraw(GraphicsCache graphics, RectangleF area)
                    {
                        // base.OnDraw(graphics, area); //Don't call the base method because this class draw the class in a different way

                        var rectangle = Rectangle.Round(area);

                        int indent = _line.Level * LevelIndent;
                        if (indent != 0)
                            rectangle = new Rectangle(rectangle.Left + indent, rectangle.Top, rectangle.Width - indent, rectangle.Height);

                        if (_line.HasChildren)
                        {
                            var image = _line.IsExpanded ? TickOpen : TickClosed;

                            graphics.Graphics.DrawImage(
                                image,
                                rectangle.X + 1,
                                rectangle.Y + (rectangle.Height - TickSize) / 2
                            );
                        }

                        if (String.IsNullOrEmpty(Value))
                            return;

                        rectangle = new Rectangle(rectangle.Left + TickSize, rectangle.Top, rectangle.Width - TickSize, rectangle.Height);

                        if (Enabled)
                            System.Windows.Forms.TextRenderer.DrawText(graphics.Graphics, Value, Font, rectangle, ForeColor, _textFormatFlags);
                        else
                            System.Windows.Forms.TextRenderer.DrawText(graphics.Graphics, Value, Font, rectangle, Color.FromKnownColor(KnownColor.GrayText), _textFormatFlags);
                    }

                    protected override SizeF OnMeasureContent(MeasureHelper measure, SizeF maxSize)
                    {
                        Size proposedSize;

                        if (maxSize != SizeF.Empty)
                        {
                            proposedSize = Size.Ceiling(maxSize);

                            //Remove the 0 because cause some auto size problems expecially when used with the EndEllipses flag.
                            if (proposedSize.Width == 0)
                                proposedSize.Width = int.MaxValue;
                            if (proposedSize.Height == 0)
                                proposedSize.Height = int.MaxValue;
                        }
                        else
                        {
                            // Declare a proposed size with dimensions set to the maximum integer value.
                            proposedSize = new Size(int.MaxValue, int.MaxValue);
                        }

                        var result = System.Windows.Forms.TextRenderer.MeasureText(measure.Graphics, Value, Font, proposedSize, _textFormatFlags);

                        return new SizeF(result.Width + _line.Level * LevelIndent + TickSize, result.Height);
                    }
                }
            }
        }
    }
}
