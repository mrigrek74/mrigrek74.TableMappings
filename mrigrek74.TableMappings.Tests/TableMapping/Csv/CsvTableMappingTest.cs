using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using mrigrek74.TableMappings.Core;
using mrigrek74.TableMappings.Core.TableMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Globalization;

namespace mrigrek74.TableMappings.Tests.TableMapping.Csv
{
    [TestClass]
    public class CsvTableMappingTest
    {
        private const string TestCsvPath = "TableMapping/Csv/Test.csv";
        private const string ValidationTestCsvPath = "TableMapping/Csv/ValidationTest.csv";
        private const string SuppressConvertTypeErrorsTestCsvPath = "TableMapping/Csv/SuppressConvertTypeErrorsTest.csv";

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var culture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        
        [TestMethod]
        public void SimpleMappingByPath()
        {
            var mapper = new CsvMapper<TestClass>(new MappingOptions
            {
                MappingMode = MappingMode.ByNumber,
                HasHeader = false
            });
            var items = mapper.Map(TestCsvPath);
            Assert.IsNotNull(items, "Result is null");
            Assert.IsTrue(items.Any(), "Items empty");

            items.SimpleMappingTrace(TestCsvPath);
        }

        [TestMethod]
        public void SimpleMappingByStream()
        {
            var mapper = new CsvMapper<TestClass>(new MappingOptions { });
            using (var fs = new FileStream(TestCsvPath, FileMode.Open))
            {
                var items = mapper.Map(fs);

                Assert.IsNotNull(items, "Result is null");
                Assert.IsTrue(items.Any(), "Result empty");

                items.SimpleMappingTrace(TestCsvPath);
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
            var mapper = new CsvMapper<ValidationTestClass>(new MappingOptions { EnableValidation = true });

            try
            {
                var items = mapper.Map(ValidationTestCsvPath);
            }
            catch (TableMappingException ex)
            {
                Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }

            Assert.Fail($"{nameof(TableMappingException)} has not been thrown");
        }


        [TestMethod]
        public void MappingWithRowLimit()
        {
            try
            {
                var mapper = new CsvMapper<TestClass>(new MappingOptions { RowsLimit = 99 });
                var items = mapper.Map(TestCsvPath);
            }
            catch (TableMappingException ex)
            {
                Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }
            Assert.Fail($"{nameof(TableMappingException)} has not been thrown");
        }

        [TestMethod]
        public void MappingWithRowLimit2()
        {
            
            var mapper = new CsvMapper<TestClass>(new MappingOptions
            {
                RowsLimit = 100,
                HasHeader = true
            });
            var items = mapper.Map(TestCsvPath);
            Assert.IsNotNull(items, "Result is null");
            Assert.IsTrue(items.Any(), "Items empty");

            items.SimpleMappingTrace(TestCsvPath);
        }


        [TestMethod]
        public void MappingWithSuppressConvertTypeErrors()
        {
            try
            {
                var mapper = new CsvMapper<TestClass>(new MappingOptions { SuppressConvertTypeErrors = false});
                var items = mapper.Map(SuppressConvertTypeErrorsTestCsvPath);
                Assert.IsNotNull(items, "Result is null");
                Assert.IsTrue(items.Any(), "Items empty");

                items.SimpleMappingTrace(TestCsvPath);
            }
            catch (TableMappingException ex)
            {
                Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }
            Assert.Fail($"{nameof(TableMappingException)} has not been thrown");
        }
    }
}