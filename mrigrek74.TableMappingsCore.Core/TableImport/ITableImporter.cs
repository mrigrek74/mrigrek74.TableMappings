using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace mrigrek74.TableMappingsCore.Core.TableImport
{
    public interface ITableImporter
    {
        void Import(string path);
        void Import(Stream stream);
        Task ImportAsync(string path, CancellationToken cancellationToken);
        Task ImportAsync(Stream stream, CancellationToken cancellationToken);
    }
}
