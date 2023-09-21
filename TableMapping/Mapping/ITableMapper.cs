using System.Collections.Generic;
using System.IO;

namespace TableMapping.Mapping
{
    public interface ITableMapper<T>
    {
        IList<T> Map(string path);
        IList<T> Map(Stream stream);
    }
}
