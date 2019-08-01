using mrigrek74.TableMappingsCore.Core.TableImport;
//using System.Data.Entity;

namespace mrigrek74.TableMappingsCore.Tests.TableImport
{
    public class TestInsertOrUpdateSaver : RowSaverBase<TestClass>
    {
        //private Entities_db;
        //private bool _needRemoveAll;

        private void RecreateDbContext()
        {
//            _db = new Entities_db();
//            _db.Configuration.AutoDetectChangesEnabled = false;
//#if DEBUG
//            _db.Database.Log = x => Trace.WriteLine(x);
//#endif
        }

        public TestInsertOrUpdateSaver()
        {
            RecreateDbContext();
        }

        //private void RemoveAllForFirstTime()
        //{
        //    if (!_needRemoveAll)
        //        return;

        //    _db.Database.ExecuteSqlCommand(
        //        "DELETE FROM tbl WHERE ID = @ID",
        //        new SqlParameter("@ID", _id));
        //    _needRemoveAll = false;
        //}


        protected override int InsertOrUpdateChunk => 100;
        protected override void ProcessImport()
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
            RecreateDbContext();
        }

        public override void Dispose()
        {
            //_db?.Dispose();
        }
    }
}
