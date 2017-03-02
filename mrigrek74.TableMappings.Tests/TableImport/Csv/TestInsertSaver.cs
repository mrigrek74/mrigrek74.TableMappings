using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using mrigrek74.TableMappings.Core.TableImport;

namespace mrigrek74.TableMappings.Tests.TableImport.Csv
{
    public class TestInsertSaver : IRowSaver<TestClass>
    {
        //private Entities _db = new Entities();
        private readonly List<TestClass> _tempList = new List<TestClass>();
        private const int InsertChunk = 25;

        public TestInsertSaver()
        {
            //_db.Configuration.AutoDetectChangesEnabled = false;
        }

        public void SaveRow(TestClass row)
        {
            _tempList.Add(row);
            if (_tempList.Count == InsertChunk)
            {
                //_db.TestClass.AddRange(_tempList);
                //_db.SaveChanges();

                Trace.WriteLine($"Saved {InsertChunk} records");
                Trace.WriteLine($"Last record: TestInt={row.TestInt};" +
                                $"TestString={row.TestString};TestDateTime={row.TestDateTime}...");

                _tempList.Clear();

                //_db.Dispose();
                //_db = new  Entities();
                //_db.Configuration.AutoDetectChangesEnabled = false;
            }
        }

        public void SaveRemainder()
        {
            //_db.TestClass.AddRange(_tempList);
            //_db.SaveChanges();

            Trace.WriteLine($"Saved {_tempList.Count} records");
            var last = _tempList.LastOrDefault();
            if (last != null)
            {
                Trace.WriteLine($"Last record: TestInt={last.TestInt};" +
                                $"TestString={last.TestString};TestDateTime={last.TestDateTime}...");}

            _tempList.Clear();
        }

        public void Dispose()
        {
            //_db?.Dispose();
        }
    }
}
