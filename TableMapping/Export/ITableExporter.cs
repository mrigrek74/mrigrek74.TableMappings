using System.Collections.Generic;
using System.IO;

namespace TableMapping.Export
{
    public interface ITableExporter<T> where T : class
    {
        void Export(IList<T> rows, Stream stream);
        void Export(IList<T> rows, string path);
    }
}
