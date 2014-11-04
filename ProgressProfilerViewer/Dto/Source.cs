using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgressProfilerViewer.Dto
{
    public class Source : ISource
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string File { get; private set; }
        public string Method { get; private set; }
        public string ListName { get; private set; }
        public string DisplayName { get; private set; }
        public int Crc { get; private set; }
        public Dictionary<int, SourceLine> Lines { get; private set; }
        public List<int> ExecutableLines { get; private set; }
        public List<Call> CallSources { get; private set; }
        public List<Call> CallTargets { get; private set; }
        public TimeSpan TotalTime { get; private set; }
        public TimeSpan TotalCumulativeTime { get; private set; }
        public TimeSpan CumulativeTime { get; private set; }
        public TimeSpan AverageTime { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public int CallCount { get; private set; }
        public double PercentagePerCall { get; private set; }
        public double Percentage { get; private set; }
        public double CumulativePercentage { get; private set; }

        public Source(int id, string name, string listName, int crc)
        {
            Id = id;
            Name = name;
            ListName = listName;
            Crc = crc;

            Lines = new Dictionary<int, SourceLine>();
            ExecutableLines = new List<int>();
            CallSources = new List<Call>();
            CallTargets = new List<Call>();

            int index = name.LastIndexOf(' ');
            if (index != -1)
            {
                Method = name.Substring(0, index);
                File = name.Substring(index + 1);
            }
            else
            {
                File = name;
            }

            DisplayName = File;
            if (Method != null)
                DisplayName += ":" + Method;
        }

        internal void UpdateStatistics1()
        {
            foreach (var line in Lines.Values)
            {
                if (StartTime == TimeSpan.Zero)
                    StartTime = line.StartTime;

                TotalTime += line.TotalTime;
                TotalCumulativeTime += line.TotalCumTime;
            }
        }

        internal void UpdateStatistics2(TimeSpan totalTime)
        {
            CallCount = CallSources.Sum(p => p.Count);
            AverageTime = new TimeSpan(TotalTime.Ticks / CallCount);
            CumulativeTime = new TimeSpan(TotalCumulativeTime.Ticks / CallCount);
            Percentage = TotalTime.TotalSeconds * 100 / totalTime.TotalSeconds;
            CumulativePercentage = TotalCumulativeTime.TotalSeconds * 100 / totalTime.TotalSeconds;
            PercentagePerCall = AverageTime.TotalSeconds * 100 / totalTime.TotalSeconds;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
