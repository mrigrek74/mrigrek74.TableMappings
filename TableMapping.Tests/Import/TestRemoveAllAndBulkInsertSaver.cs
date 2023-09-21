//using System.Collections.Generic;
//using TableMapping.Extensions;
//using TableMapping.Import;

//namespace TableMapping.Tests.Import
//{
//    public class TestRemoveAllAndBulkInsertSaver : IRowSaver<TestClass>
//    {
//        //private Entities _db = new Entities(); 
//        private readonly List<TestClass> _tempList = new List<TestClass>();
//        private const int BulkInsertChunk = 1_000;
//        private bool _needRemoveAll = true;

//        public TestRemoveAllAndBulkInsertSaver()
//        {
//            //_db.Configuration.AutoDetectChangesEnabled = false;
//        }

//        private void RemoveAllForFirstTime()
//        {
//            if (!_needRemoveAll)
//                return;

//            //_db.Database.ExecuteSqlCommand(
//            //    "DELETE FROM [dbo].[TestClass] WHERE ... = @...",
//            //    new SqlParameter("@...", ...));
//            _needRemoveAll = false;
//        }

//        private void BulkInsert()
//        {
//            //var bulkCopy = new SqlBulkCopy("")
//            //{
//            //    DestinationTableName = "[dbo].[TestClass]"
//            //};

//            var table = _tempList.ToDataTable();
//            //bulkCopy.WriteToServer(table);
//        }


//        public void SaveRow(TestClass row)
//        {
//            _tempList.Add(row);
//            if (_tempList.Count == BulkInsertChunk)
//            {
//                RemoveAllForFirstTime();
//                BulkInsert();
//                _tempList.Clear();
//            }
//        }

//        public void SaveRemainder()
//        {
//            RemoveAllForFirstTime();
//            BulkInsert();
//            _tempList.Clear();
//        }

//        public void Dispose()
//        {
//            //_db?.Dispose();
//        }
//    }
//}
