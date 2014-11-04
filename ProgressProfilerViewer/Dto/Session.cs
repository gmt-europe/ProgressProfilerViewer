using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ProgressProfilerViewer.Dto
{
    public class Session
    {
        public static Session Load(string fileName)
        {
            using (var reader = File.OpenText(fileName))
            {
                return Load(new DataReader(reader));
            }
        }

        private static Session Load(DataReader reader)
        {
            if (!reader.Read())
                return null;

            int lastSourceId = 0;

            var session = new Session(
                (int)reader[0],
                (DateTime)reader[1],
                (string)reader[2],
                (TimeSpan)reader[3],
                (string)reader[4]
            );

            if (reader.Read() || !reader.Next())
                return null;

            while (reader.Read())
            {
                var source = new Source(
                    (int)reader[0],
                    (string)reader[1],
                    (string)reader[2],
                    (int)reader[3]
                );

                if (!session.Sources.ContainsKey(source.Id))
                {
                    session.Sources.Add(source.Id, source);

                    lastSourceId = Math.Max(lastSourceId, source.Id);
                }
            }

            if (!reader.Next())
                return null;

            var callTreeDatas = new List<CallTreeData>();

            while (reader.Read())
            {
                callTreeDatas.Add(new CallTreeData(
                    (int)reader[0],
                    (int)reader[1],
                    (int)reader[2],
                    (int)reader[3]
                ));
            }

            if (!reader.Next())
                return null;

            SourceLine summaryLine = null;

            while (reader.Read())
            {
                int id = (int)reader[0];

                var sourceLine = new SourceLine(
                    (int)reader[1],
                    (int)reader[2],
                    ToTicks((decimal)reader[3]),
                    ToTicks((decimal)reader[4])
                );
                
                Source source;
                if (!session.Sources.TryGetValue(id, out source))
                {
                    Debug.Assert(id == 0);
                    summaryLine = sourceLine;
                    continue;
                }

                session.Sources[id].Lines.Add(
                    (int)reader[1],
                    sourceLine
                );
            }

            if (!reader.Next())
                return null;

            if (session.VersionNumber >= 1)
            {
                while (reader.Read())
                {
                    session.Sources[(int)reader[0]].Lines[(int)reader[1]].AddTrace(
                        ToTicks((decimal)reader[2]),
                        ToTicks((decimal)reader[3])
                    );
                }

                if (!reader.Next())
                    return null;
            }

            while (true)
            {
                if (!reader.Read())
                    break;

                Source source;
                int sourceId = (int)reader[0];
                string sourceName = (string)reader[1];

                if (!String.IsNullOrEmpty(sourceName))
                {
                    Source parentSource;
                    if (!session.Sources.TryGetValue(sourceId, out parentSource))
                        continue;

                    sourceName = parentSource.Name + " " + sourceName;

                    source = session.Sources.Values.SingleOrDefault(p => p.Name == sourceName);
                    if (source == null)
                    {
                        source = new Source(
                            ++lastSourceId,
                            sourceName,
                            null,
                            0
                        );

                        session.Sources.Add(source.Id, source);
                    }
                }
                else
                {
                    source = session.Sources[sourceId];
                }

                while (reader.Read())
                {
                    source.ExecutableLines.Add((int)reader[0]);
                }

                if (!reader.Next())
                    return null;
            }

            // Process the call tree and put them on the sources.

            foreach (var data in callTreeDatas)
            {
                Source caller;
                if (!session.Sources.TryGetValue(data.Caller, out caller))
                    Debug.Assert(data.Caller == 0);

                Source callee;
                if (!session.Sources.TryGetValue(data.Callee, out callee))
                    Debug.Assert(data.Callee == 0);

                Debug.Assert(caller != null || callee != null);

                var call = new Call(data.Line, data.Count, callee, caller);

                if (caller != null)
                    caller.CallTargets.Add(call);
                if (callee != null)
                    callee.CallSources.Add(call);
            }

            // Trim all unnecessary sources.

            Debug.Assert(summaryLine != null);

            session.TotalTime = summaryLine.CumTime;

            foreach (int id in session.Sources.Keys.ToArray())
            {
                var source = session.Sources[id];

                // It seems that sources that were not hit during the run are still included
                // in the profile session. These can be removed.

                if (source.CallSources.Count == 0 && source.CallTargets.Count == 0)
                {
                    Debug.Assert(source.Lines.Count == 0);
                    session.Sources.Remove(id);
                }
            }

            // Calculate the total session time.

            long totalTime = 0;

            foreach (var source in session.Sources.Values)
            {
                source.UpdateStatistics1();

                totalTime += source.TotalTime.Ticks;
            }

            session.TotalTime = new TimeSpan(totalTime);

            // Update the source statistics.

            foreach (var source in session.Sources.Values)
            {
                source.UpdateStatistics2(session.TotalTime);
            }

            // Find the root source.

            session.RootSource = session.Sources[session.Sources.Keys.Min()];

            Debug.Assert(session.RootSource.Id == 1);

            return session;
        }

        private static long ToTicks(decimal seconds)
        {
            return (long)(TimeSpan.TicksPerSecond * seconds);
        }

        public int VersionNumber { get; private set; }
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public string User { get; private set; }
        public Dictionary<int, Source> Sources { get; private set; }
        public Source RootSource { get; private set; }
        public TimeSpan TotalTime { get; private set; }

        private Session(int versionNumber, DateTime date, string description, TimeSpan time, string user)
        {
            VersionNumber = versionNumber;
            Date = date.Add(time);
            Description = description;
            User = user;

            Sources = new Dictionary<int, Source>();
        }

        private class CallTreeData
        {
            public int Caller { get; private set; }
            public int Line { get; private set; }
            public int Callee { get; private set; }
            public int Count { get; private set; }

            public CallTreeData(int caller, int line, int callee, int count)
            {
                Caller = caller;
                Line = line;
                Callee = callee;
                Count = count;
            }
        }
    }
}
