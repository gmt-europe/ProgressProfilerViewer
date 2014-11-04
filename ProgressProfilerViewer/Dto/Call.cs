using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgressProfilerViewer.Dto
{
    public class Call
    {
        public int Line { get; private set; }
        public int Count { get; private set; }
        public Source Callee { get; private set; }
        public Source Caller { get; private set; }

        public Call(int line, int count, Source callee, Source caller)
        {
            Line = line;
            Count = count;
            Callee = callee;
            Caller = caller;
        }

        public override string ToString()
        {
            return String.Format("{0} -> {1}", Caller, Callee);
        }
    }
}
