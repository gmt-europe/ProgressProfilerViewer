using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ProgressProfilerViewer.Dto
{
    public class DataReader
    {
        private readonly StreamReader _reader;
        private bool _haveNext;
        private readonly List<object> _values = new List<object>();

        public int Line { get; private set; }

        public DataReader(StreamReader reader)
        {
            _reader = reader;
        }

        public object this[int index]
        {
            get { return _values[index]; }
        }

        public int ValueCount
        {
            get { return _values.Count; }
        }

        public bool Read()
        {
            _values.Clear();
            Line++;
            string line = _reader.ReadLine();

            if (line == null)
                return false;
            if (line == ".")
            {
                _haveNext = true;
                return false;
            }

            ParseLine(line);

            return true;
        }

        private void ParseLine(string line)
        {
            while (line != null)
            {
                if (line.StartsWith("? ") || line == "?")
                {
                    _values.Add(null);

                    line = line.Length == 1 ? null : line.Substring(2);
                }
                else
                {
                    _values.Add(ParseValue(ref line));
                }
            }
        }

        private object ParseValue(ref string line)
        {
            if (line[0] == '"')
                return ParseCharacter(ref line);
            if (Char.IsDigit(line[0]))
                return ParseDecimal(ref line);
            return ParseLogical(ref line);
        }

        private object ParseLogical(ref string line)
        {
            string[] parts = line.Split(new char[] { ' ' }, 2);

            line = parts.Length == 2 ? parts[1] : null;

            switch (parts[0])
            {
                case "yes":
                    return true;

                case "no":
                    return false;

                default:
                    throw new InvalidOperationException("Could not parse line");
            }
        }

        private object ParseDecimal(ref string line)
        {
            string[] parts = line.Split(new[] { ' ' }, 2);

            line = parts.Length == 2 ? parts[1] : null;

            string value = parts[0].Replace(',', '.');

            if (value.IndexOf('/') != -1)
                return ParseDate(value);
            if (value.IndexOf(':') != -1)
                return ParseTime(value);
            if (value.IndexOf('.') == -1)
                return int.Parse(value, CultureInfo.InvariantCulture);

            if (value[0] == '.')
                value = "0" + value;
            else if (value.StartsWith("-."))
                value = "-0." + value.Substring(2);

            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private object ParseTime(string value)
        {
            string[] timeParts = value.Split(':');

            if (timeParts.Length != 3)
                throw new InvalidOperationException("Could not parse line");

            return new TimeSpan(
                int.Parse(timeParts[0], CultureInfo.InvariantCulture),
                int.Parse(timeParts[1], CultureInfo.InvariantCulture),
                int.Parse(timeParts[2], CultureInfo.InvariantCulture)
            );
        }

        private object ParseDate(string value)
        {
            string[] dateParts = value.Split('/');

            if (dateParts.Length != 3)
                throw new InvalidOperationException("Could not parse line");

            int year = int.Parse(dateParts[2], CultureInfo.InvariantCulture);

            if (year > 3000)
                return DateTime.MaxValue;
            if (year < 100)
                year += year > 50 ? 1900 : 2000;

            return new DateTime(year, int.Parse(dateParts[1], CultureInfo.InvariantCulture), int.Parse(dateParts[0], CultureInfo.InvariantCulture));
        }

        private object ParseCharacter(ref string line)
        {
            var sb = new StringBuilder();

            bool hadOne = false;

            while (line != null && line[0] == '"')
            {
                int pos = line.IndexOf('"', 1);

                while (pos == -1)
                {
                    string newLine = _reader.ReadLine();

                    if (newLine == null)
                        throw new InvalidOperationException("Could not parse line");

                    line += "\n" + newLine;

                    pos = line.IndexOf('"', 1);
                }

                if (hadOne)
                    sb.Append('"');

                sb.Append(line.Substring(1, pos - 1));

                line = line.Substring(pos + 1);

                hadOne = true;

                if (line.Length == 0)
                    line = null;
            }

            if (line != null)
            {
                if (!char.IsWhiteSpace(line[0]))
                    throw new InvalidOperationException("Could not parse line");

                line = line.Substring(1);
            }

            return sb.ToString();
        }

        public bool Next()
        {
            if (_haveNext)
            {
                _haveNext = false;
                return true;
            }

            return false;
        }
    }
}
