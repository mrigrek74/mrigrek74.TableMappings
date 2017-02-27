using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace mrigrek74.TableMappings.Core.TableMapping.Import
{
    public interface ITableImporter
    {
          event EventHandler<DocumentImportEventArgs> Progress;

        void Import(string path);
        void Import(Stream stream);
        Task ImportAsync(string path, CancellationToken cancellationToken);
        Task ImportAsync(Stream stream, CancellationToken cancellationToken);
    }
}
