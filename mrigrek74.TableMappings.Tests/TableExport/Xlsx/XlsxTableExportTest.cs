using System;
using System.Collections.Generic;
using System.IO;
using mrigrek74.TableMappings.Core.Epplus.TableExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mrigrek74.TableMappings.Tests.TableExport.Xlsx
{
    [TestClass]
    public class XlsxTableExportTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string fname = "Exported.xlsx";
            var list = new List<TestClass>
            {
                new TestClass
                {
                    TestInt = 1,
                    TestGuid = Guid.NewGuid(),
                    TestFloat = 2.1f,
                    TestDateTime = DateTime.Now
                },
                new TestClass
                {
                    TestInt = 2,
                    TestGuid = Guid.NewGuid(),
                    TestFloat = 2.2f,
                    TestDateTime = DateTime.Now
                }
            };

            var exporter = new XlsxTableExporter<TestClass>();
            exporter.Export(list, fname);
            if (!File.Exists(fname))
                Assert.Fail($"{fname} is not exists");
        }
    }
}
