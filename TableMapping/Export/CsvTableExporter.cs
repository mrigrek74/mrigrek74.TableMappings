using System;
using System.Collections.Generic;
using System.IO;
using TableMapping.Extensions;

namespace TableMapping.Export
{
    public class CsvTableExporter<T> : ITableExporter<T> where T : class
    {
        private readonly char _delimiter;

        private void WriteRows(IList<T> rows, TextWriter sr)
        {
            Type t = typeof(T);
            var props = t.GetProperties();
            foreach (var p in props)
            {
                var colName = p.GetColumnName();
                sr.Write(colName.ToCsvFormat(_delimiter) + _delimiter);
            }
            sr.WriteLine();

            foreach (var row in rows)
            {
                foreach (var p in props)
                {
                    var val = p.GetValue(row, null);
                    var valStr = (val ?? string.Empty).ToString();
                    sr.Write(valStr.ToCsvFormat(_delimiter) + _delimiter);
                }
                sr.WriteLine();
            }
        }

        public CsvTableExporter()
        {
            _delimiter = ';';
        }

        public CsvTableExporter(char delimiter)
        {
            _delimiter = delimiter;
        }

        public void Export(IList<T> rows, string path)
        {
            using (var sr = new StreamWriter(path))
            {
                WriteRows(rows, sr);
            }
        }

        public void Export(IList<T> rows, Stream stream)
        {
            using (var sr = new StreamWriter(stream))
            {
                WriteRows(rows, sr);
            }
        }
    }
}