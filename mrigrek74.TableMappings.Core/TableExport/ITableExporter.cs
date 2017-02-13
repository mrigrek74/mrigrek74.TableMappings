using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace mrigrek74.TableMappings.Core.TableExport
{
    public interface ITableExporter<T> where T : class
    {
        void Export(IList<T> rows, Stream stream);
        void Export(IList<T> rows, string path);
    }
}
