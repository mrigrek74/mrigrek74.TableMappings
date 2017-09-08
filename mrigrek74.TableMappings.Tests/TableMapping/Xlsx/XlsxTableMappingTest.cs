using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using mrigrek74.TableMappings.Core;
using mrigrek74.TableMappings.Core.Epplus.TableMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mrigrek74.TableMappings.Tests.TableMapping.Xlsx
{
    [TestClass]
    public class XlsxTableMappingTest
    {
        private const string TestXlsxPath = "TableMapping/Xlsx/Test.xlsx";
        private const string ValidationTestXlsxPath = "TableMapping/Xlsx/ValidationTest.xlsx";
        private const string SuppressConvertTypeErrorsTestXlsxPath
            = "TableMapping/Xlsx/SuppressConvertTypeErrorsTest.xlsx";

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        private void SimpleMappingTrace(ICollection<TestClass> items, string path)
        {
            Trace.WriteLine(path);
            Trace.WriteLine("");
            const int take = 100;
            Trace.WriteLine($"First {take} of {items.Count}:");

            var top = items.Take(take).ToList();

            for (int i = 0; i < top.Count; i++)
            {
                var item = top[i];
                if (i == 0)
                {
                    foreach (var p in item.GetType().GetProperties())
                    {
                        Trace.Write($"{p.Name};");
                    }
                    Trace.WriteLine(string.Empty);
                }
                foreach (var p in item.GetType().GetProperties())
                {
                    Trace.Write($"{p.GetValue(item, null)};");
                }
                Trace.WriteLine(string.Empty);
            }
        }


        [TestMethod]
        public void SimpleMappingByPath()
        {
            var mapper = new XlsxMapper<TestClass>(MappingMode.ByName);
            var items = mapper.Map(TestXlsxPath);
            Assert.IsNotNull(items, "Result is null");
            Assert.IsTrue(items.Any(), "items empty");

            SimpleMappingTrace(items, TestXlsxPath);
        }

        [TestMethod]
        public void SimpleMappingByStream()
        {
            var mapper = new XlsxMapper<TestClass>(MappingMode.ByName);
            using (var fs = new FileStream(TestXlsxPath, FileMode.Open))
            {
                var items = mapper.Map(fs);

                Assert.IsNotNull(items, "Result is null");
                Assert.IsTrue(items.Any(), "items empty");

                SimpleMappingTrace(items, TestXlsxPath);
            }
        }

        public class ValidationTestClass
        {
            [ColumnName("Test String")]
            [MaxLength(10)]
            public string TestString { get; set; }
            [ColumnName("Test Int")]
            public int TestInt { get; set; }
        }

        [TestMethod]
        public void MappingWithValidation()
        {
            var mapper = new XlsxMapper<ValidationTestClass>(MappingMode.ByName, true);

            try
            {
                var items = mapper.Map(ValidationTestXlsxPath);
            }
            catch (TableMappingException ex)
            {
                Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }

            Assert.Fail("TableMappingException has not been thrown");
        }


        [TestMethod]
        public void MappingWithRowLimit()
        {
            try
            {
                var mapper = new XlsxMapper<TestClass>(MappingMode.ByName, false, true, 99);
                var items = mapper.Map(TestXlsxPath);
            }
            catch (TableMappingException ex)
            {
                Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }
            Assert.Fail("TableMappingException has not been thrown");
        }

        [TestMethod]
        public void MappingWithRowLimit2()
        {
            var mapper = new XlsxMapper<TestClass>(MappingMode.ByName, false, true, 100);
            var items = mapper.Map(TestXlsxPath);
            Assert.IsNotNull(items, "Result is null");
            Assert.IsTrue(items.Any(), "items empty");

            SimpleMappingTrace(items, TestXlsxPath);
        }

        [TestMethod]
        public void MappingWithSuppressConvertTypeErrors()
        {
            try
            {
                var mapper = new XlsxMapper<TestClass>(MappingMode.ByName, false, false, null);
                var items = mapper.Map(SuppressConvertTypeErrorsTestXlsxPath);
                Assert.IsNotNull(items, "Result is null");
                Assert.IsTrue(items.Any(), "items empty");

                SimpleMappingTrace(items, TestXlsxPath);
            }
            catch (TableMappingException ex)
            {
                Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }
            Assert.Fail("TableMappingException has not been thrown");
        }
    }
}