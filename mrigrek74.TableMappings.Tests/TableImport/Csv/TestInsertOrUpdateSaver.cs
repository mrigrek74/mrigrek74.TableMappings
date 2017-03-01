using System.Collections.Generic;
//using System.Data.Entity;
using mrigrek74.TableMappings.Core.TableImport;

namespace mrigrek74.TableMappings.Tests.TableImport.Csv
{
    public class TestInsertOrUpdateSaver : IRowSaver<TestClass>
    {
        //private Entities _db = new Entities(); 
        private readonly List<TestClass> _tempList = new List<TestClass>();
        private const int InsertOrUpdateChunk = 500;

        private void ProcessInsertOrUpdate()
        {
            //var ids = _tempList
            //    .Where(x => x.LocalObjectId.HasValue)
            //    .Select(x => x.LocalObjectId)
            //    .ToList();

            //if (ids.Any())
            //{
            //    var existingList = _db.TestClass
            //        .Where(x =>
            //            x.CustId == _custId && x.LocalObjectId.HasValue && ids.Contains(x.LocalObjectId))
            //        .Select(x => new { x.Id, LocalObjectId = x.LocalObjectId.Value })
            //        .ToList();

            //    foreach (var addr in _tempList)
            //    {
            //        var existing = existingList.SingleOrDefault(x => x.LocalObjectId == addr.LocalObjectId);
            //        if (existing != null)
            //        {
            //            addr.Id = existing.Id;
            //            _db.TestClass.Attach(addr);
            //            _db.Entry(addr).State = EntityState.Modified;
            //        }
            //        else
            //        {
            //            _db.Entry(addr).State = EntityState.Added;
            //        }
            //    }
            //}
            //else
            //{

            //    foreach (var addr in _tempList)
            //    {
            //        _db.TestClass.Attach(addr);
            //        _db.Entry(addr).State = EntityState.Added;
            //    }

            //}

            //_db.SaveChanges();
            _tempList.Clear();
        }

        public void SaveRow(TestClass row)
        {
            _tempList.Add(row);
            if (_tempList.Count == InsertOrUpdateChunk)
            {
                ProcessInsertOrUpdate();

                //_db.Dispose();
                //_db = new Entities();
                //_db.Configuration.AutoDetectChangesEnabled = false;
            }
        }

        public void SaveRemainder()
        {
            ProcessInsertOrUpdate();
        }

        public void Dispose()
        {
            //_db?.Dispose();
        }
    }
}
