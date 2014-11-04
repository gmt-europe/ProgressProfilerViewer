using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgressProfilerViewer
{
    public interface ISource
    {
        string DisplayName { get; }
        int CallCount { get; }
        double Percentage { get; }
        double CumulativePercentage { get; }
        TimeSpan AverageTime { get; }
        TimeSpan CumulativeTime { get; }
    }
}
