using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using TableMapping.Import;
using System;

namespace TableMapping.Tests.Import
{
    public class TestInsertSaver : RowSaverBase<TestClass>
    {
        //private Entities _db = new Entities();
        public Action<string> Log;

        public TestInsertSaver()
        {
            //_db.Configuration.AutoDetectChangesEnabled = false;
        }

        protected override int InsertOrUpdateChunk => 25;
        protected override async Task ProcessImportAsync(CancellationToken ct)
        {
            Trace.WriteLine($"Saved {TempList.Count} records");
            var last = TempList.LastOrDefault();
            if (last != null)
            {
                Log?.Invoke($"TotalImported:{TotalImported + TempList.Count} TestInt={last.TestInt};" +
                               $"TestString={last.TestString};TestDateTime={last.TestDateTime}..."
                );
            }


        }

        public override void Dispose()
        {
            //_db?.Dispose();
        }
    }
}
