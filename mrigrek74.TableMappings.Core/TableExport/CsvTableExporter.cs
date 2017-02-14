using System;
using System.Collections.Generic;
using System.IO;

namespace mrigrek74.TableMappings.Core.TableExport
{
    public class CsvTableExporter<T> : ITableExporter<T> where T : class
    {
        private readonly char _delimiter;

        private void WriteRows(IList<T> rows, TextWriter sr)
        {
            Type t = typeof(T);
            var props = t.GetProperties();
            for (int j = 0; j < props.Length; j++)
            {
                var colName = ColumnNameAttribute.GetColumnName(t, props[j]);
                sr.Write(colName.ToCsvFormat(_delimiter) + _delimiter);
            }
            sr.WriteLine();

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < props.Length; j++)
                {
                    var val = props[j].GetValue(rows[i], null);
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