using System.Diagnostics;
using System.Linq;
using mrigrek74.TableMappingsCore.Core.TableImport;

namespace mrigrek74.TableMappingsCore.Tests.TableImport
{
    public class TestInsertSaver : RowSaverBase<TestClass>
    {
        //private Entities _db = new Entities();

        public TestInsertSaver()
        {
            //_db.Configuration.AutoDetectChangesEnabled = false;
        }

        protected override int InsertOrUpdateChunk => 25;
        protected override void ProcessImport()
        {
            Trace.WriteLine($"Saved {TempList.Count} records");
            var last = TempList.LastOrDefault();
            if (last != null)
            {
                Trace.WriteLine($"Last record: TestInt={last.TestInt};" +
                                $"TestString={last.TestString};TestDateTime={last.TestDateTime}...");
            }
        }

        public override void Dispose()
        {
            //_db?.Dispose();
        }
    }
}
