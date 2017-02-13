using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mrigrek74.TableMappings.Core.TableExport
{
    public class CsvTableExporter<T> : ITableExporter<T> where T : class
    {
        private readonly Encoding _encoding;
        private readonly string[] _delimiters;

        public CsvTableExporter()
        {
            _encoding = Encoding.UTF8;
            _delimiters = new[] { ";" };
        }

        public CsvTableExporter(string[] delimiters)
        {
            _encoding = Encoding.UTF8;
            _delimiters = delimiters;
        }

        public CsvTableExporter(string[] delimiters, Encoding encoding)
        {
            _encoding = encoding;
            _delimiters = delimiters;
        }

        public void Export(IList<T> rows, string path)
        {
            throw new NotImplementedException();
        }

        public void Export(IList<T> rows, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
