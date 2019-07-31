using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using mrigrek74.TableMappingsCore.Core.TableExport;
using mrigrek74.TableMappingsCore.Tests;
using Xunit;

namespace mrigrek74.TableMappingsCore.Tests.TableExport.Csv
{
    
    public class CsvTableExportTest
    {

        public  CsvTableExportTest()
        {
            var culture = new CultureInfo(TestConstants.Culture);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        [Fact]
        public void Method1()
        {
            const string fname = "Exported.csv";
            var list = new List<TestClass>
            {
                new TestClass
                {
                    TestString = "@@",
                    TestInt = 1,
                    TestGuid = new Guid("a6f6f6fd-7687-48a5-8e9e-c9ce3ae7f093"),
                    TestFloat = 2.1f,
                    TestDateTime = DateTime.Parse("04.07.2017 10:35:17")
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
            exporter.Export(list, fname);

            if (!File.Exists(fname))
                throw new Exception($"{fname} is not exists");

            var lines = File.ReadAllLines(fname);

            Assert.NotEmpty(lines);

            Assert.Equal(3, lines.Length);

            Assert.Equal("Test String;Test Int;Test Int?;Test Float;Test Float?;Test Decimal;Test Decimal?;" +
                            "Test Double;Test Double?;Test Guid;Test Guid?;Test DateTime;Test DateTime?;", lines[0]);
            Assert.Equal("@@;1;;2,1;;0;;0;;a6f6f6fd-7687-48a5-8e9e-c9ce3ae7f093;;04.07.2017 10:35:17;;", lines[1]);
        }
    }
}
