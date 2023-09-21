using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TableMapping.Import
{
    public interface ITableImporter
    {
        Task ImportAsync(string path, CancellationToken cancellationToken);
        Task ImportAsync(Stream stream, CancellationToken cancellationToken);
    }
}
