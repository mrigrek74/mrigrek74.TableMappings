using System;
using System.Collections.Generic;
using System.IO;
using TableMapping.EPPlus.Export;
using Xunit;

namespace TableMapping.Tests.Export.Xlsx
{

    public class XlsxTableExportTest
    {
        [Fact]
        public void Method1()
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
                throw new Exception($"{fname} is not exists");
        }
    }
}
