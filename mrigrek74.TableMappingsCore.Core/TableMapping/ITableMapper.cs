using System.Collections.Generic;
using System.IO;

namespace mrigrek74.TableMappingsCore.Core.TableMapping
{
    public interface ITableMapper<T>
    {
        IList<T> Map(string path);
        IList<T> Map(Stream stream);
    }
}
