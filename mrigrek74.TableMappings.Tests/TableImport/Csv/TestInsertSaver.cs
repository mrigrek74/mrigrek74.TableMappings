using System.Collections.Generic;
using mrigrek74.TableMappings.Core.TableImport;

namespace mrigrek74.TableMappings.Tests.TableImport.Csv
{
    public class TestInsertSaver : IRowSaver<TestClass>
    {
        //private Entities _db = new Entities();
        private readonly List<TestClass> _tempList = new List<TestClass>();
        private const int InsertChunk = 500;

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
            _tempList.Clear();
        }

        public void Dispose()
        {
            //_db?.Dispose();
        }
    }
}
