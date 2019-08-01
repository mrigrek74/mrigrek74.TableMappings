using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using mrigrek74.TableMappingsCore.Core;
using mrigrek74.TableMappingsCore.Core.TableMapping;
using System.Threading;
using System.Globalization;
using Xunit;
using Xunit.Abstractions;

namespace mrigrek74.TableMappingsCore.Tests.TableMapping.Csv
{

    public class CsvTableMappingTest
    {
        private const string TestCsvPath = "TableMapping/Csv/Test.csv";
        private const string ValidationTestCsvPath = "TableMapping/Csv/ValidationTest.csv";
        private const string SuppressConvertTypeErrorsTestCsvPath = "TableMapping/Csv/SuppressConvertTypeErrorsTest.csv";

        private readonly ITestOutputHelper _output;
        public CsvTableMappingTest(ITestOutputHelper output)
        {
            _output = output;
            var culture = new CultureInfo(TestConstants.Culture);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        [Fact]
        public void SimpleMappingByPath()
        {
            var mapper = new CsvMapper<TestClass>(new MappingOptions
            {
                MappingMode = MappingMode.ByNumber,
                HasHeader = false
            });
            var items = mapper.Map(TestCsvPath);
            Assert.NotNull(items);
            Assert.True(items.Any(), "Items empty");

            items.SimpleMappingTrace(TestCsvPath, _output);
        }

        [Fact]
        public void SimpleMappingByStream()
        {
            var mapper = new CsvMapper<TestClass>(new MappingOptions { });
            using (var fs = new FileStream(TestCsvPath, FileMode.Open))
            {
                var items = mapper.Map(fs);

                Assert.NotNull(items);
                Assert.True(items.Any(), "Result empty");

                items.SimpleMappingTrace(TestCsvPath, _output);
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

        [Fact]
        public void MappingWithValidation()
        {
            var mapper = new CsvMapper<ValidationTestClass>(new MappingOptions { EnableValidation = true });

            try
            {
                var items = mapper.Map(ValidationTestCsvPath);
            }
            catch (TableMappingException ex)
            {
                _output.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }

            throw new Exception($"{nameof(TableMappingException)} has not been thrown");
        }


        [Fact]
        public void MappingWithRowLimit()
        {
            try
            {
                var mapper = new CsvMapper<TestClass>(new MappingOptions { RowsLimit = 99 });
                var items = mapper.Map(TestCsvPath);
            }
            catch (TableMappingException ex)
            {
                _output.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }
            throw new Exception($"{nameof(TableMappingException)} has not been thrown");
        }

        [Fact]
        public void MappingWithRowLimit2()
        {
            
            var mapper = new CsvMapper<TestClass>(new MappingOptions
            {
                RowsLimit = 100,
                HasHeader = true
            });
            var items = mapper.Map(TestCsvPath);
            Assert.NotNull(items);
            Assert.True(items.Any(), "Items empty");

            items.SimpleMappingTrace(TestCsvPath, _output);
        }


        [Fact]
        public void MappingWithSuppressConvertTypeErrors()
        {
            try
            {
                var mapper = new CsvMapper<TestClass>(new MappingOptions { SuppressConvertTypeErrors = false});
                var items = mapper.Map(SuppressConvertTypeErrorsTestCsvPath);
                Assert.NotNull(items);
                Assert.True(items.Any(), "Items empty");

                items.SimpleMappingTrace(TestCsvPath, _output);
            }
            catch (TableMappingException ex)
            {
                _output.WriteLine($"{ex.Message}; Row {ex.Row}");
                return;
            }
            throw new Exception($"{nameof(TableMappingException)} has not been thrown");
        }
    }
}