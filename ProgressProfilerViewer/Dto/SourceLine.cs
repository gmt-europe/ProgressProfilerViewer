using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgressProfilerViewer.Dto
{
    public class SourceLine
    {
        public int Line { get; private set; }
        public int Count { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public TimeSpan TotalCumTime { get; private set; }
        public TimeSpan CumTime { get; private set; }
        public TimeSpan AverageTime { get; private set; }
        public TimeSpan Time { get; private set; }
        public TimeSpan StartTime { get; private set; }

        public SourceLine(int line, int count, long totalTime, long cumTime)
        {
            Line = line;
            Count = count;
            TotalTime = new TimeSpan(totalTime);
            TotalCumTime = new TimeSpan(cumTime);
            AverageTime = new TimeSpan(totalTime / count);
            CumTime = new TimeSpan(cumTime / count);
        }

        public void AddTrace(long time, long startTime)
        {
            Time = new TimeSpan(time);
            StartTime = new TimeSpan(startTime);
        }

        public override string ToString()
        {
            return Line.ToString();
        }
    }
}
