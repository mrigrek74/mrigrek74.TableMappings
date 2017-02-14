using System;
using System.Collections.Generic;
using mrigrek74.TableMappings.Core.Epplus.TableExport;
using mrigrek74.TableMappings.Core.TableExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mrigrek74.TableMappings.Tests.TableExport.Csv
{
    [TestClass]
    public class CsvTableExportTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var list = new List<TestClass>
            {
                new TestClass
                {
                    TestString = "@@",
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

            var exporter = new CsvTableExporter<TestClass>();
            exporter.Export(list, "Exported.csv");
        }
    }
}
