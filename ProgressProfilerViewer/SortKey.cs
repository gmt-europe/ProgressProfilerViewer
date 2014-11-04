using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgressProfilerViewer
{
    public class SortKey
    {
        public int Column { get; private set; }
        public bool Ascending { get; private set; }

        public SortKey(int column, bool ascending)
        {
            Column = column;
            Ascending = ascending;
        }
    }
}
