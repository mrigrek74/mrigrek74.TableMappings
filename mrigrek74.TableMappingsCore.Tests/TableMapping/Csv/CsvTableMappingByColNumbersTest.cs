﻿using System.Linq;
using mrigrek74.TableMappingsCore.Core;
using mrigrek74.TableMappingsCore.Core.TableMapping;
using System.Threading;
using System.Globalization;
using Xunit;
using Xunit.Abstractions;

namespace mrigrek74.TableMappingsCore.Tests.TableMapping.Csv
{
    public class CsvTableMappingByColNumbersTest
    {
        private const string TestCsvPath = "TableMapping/Csv/Test.csv";
        private const string ValidationTestCsvPath = "TableMapping/Csv/ValidationTest.csv";
        private const string SuppressConvertTypeErrorsTestCsvPath = "TableMapping/Csv/SuppressConvertTypeErrorsTest.csv";

        private readonly ITestOutputHelper _output;
        public CsvTableMappingByColNumbersTest(ITestOutputHelper output)
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
            var mapper = new CsvMapper<TestClass>(
                new MappingOptions
            {
                MappingMode = MappingMode.ByNumber,
                SuppressConvertTypeErrors = false,
                EnableValidation = true,
                HasHeader = true
            });
            var items = mapper.Map(TestCsvPath);
            Assert.NotNull(items);
            Assert.True(items.Any(), "Items empty");

            items.SimpleMappingTrace(TestCsvPath, _output);
        }

        //[TestMethod]
        //public void SimpleMappingByStream()
        //{
        //    var mapper = new CsvMapper<TestClass>(MappingMode.ByName);
        //    using (var fs = new FileStream(TestCsvPath, FileMode.Open))
        //    {
        //        var items = mapper.Map(fs);

        //        Assert.IsNotNull(items, "Result is null");
        //        Assert.IsTrue(items.Any(), "Result empty");

        //        SimpleMappingTrace(items, TestCsvPath);
        //    }
        //}


        //public class ValidationTestClass
        //{
        //    [ColumnName("Test String")]
        //    [MaxLength(10)]
        //    public string TestString { get; set; }
        //    [ColumnName("Test Int")]
        //    public int TestInt { get; set; }
        //}

        //[TestMethod]
        //public void MappingWithValidation()
        //{
        //    var mapper = new CsvMapper<ValidationTestClass>(MappingMode.ByName, true);

        //    try
        //    {
        //        var items = mapper.Map(ValidationTestCsvPath);
        //    }
        //    catch (TableMappingException ex)
        //    {
        //        Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
        //        return;
        //    }

        //    Assert.Fail($"{nameof(TableMappingException)} has not been thrown");
        //}


        //[TestMethod]
        //public void MappingWithRowLimit()
        //{
        //    try
        //    {
        //        var mapper = new CsvMapper<TestClass>(MappingMode.ByName,false, true, 99);
        //        var items = mapper.Map(TestCsvPath);
        //    }
        //    catch (TableMappingException ex)
        //    {
        //        Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
        //        return;
        //    }
        //    Assert.Fail($"{nameof(TableMappingException)} has not been thrown");
        //}

        //[TestMethod]
        //public void MappingWithRowLimit2()
        //{
        //    var mapper = new CsvMapper<TestClass>(MappingMode.ByName,false, true, 100);
        //    var items = mapper.Map(TestCsvPath);
        //    Assert.IsNotNull(items, "Result is null");
        //    Assert.IsTrue(items.Any(), "Items empty");

        //    SimpleMappingTrace(items, TestCsvPath);
        //}


        //[TestMethod]
        //public void MappingWithSuppressConvertTypeErrors()
        //{
        //    try
        //    {
        //        var mapper = new CsvMapper<TestClass>(MappingMode.ByName,false, false, null);
        //        var items = mapper.Map(SuppressConvertTypeErrorsTestCsvPath);
        //        Assert.IsNotNull(items, "Result is null");
        //        Assert.IsTrue(items.Any(), "Items empty");

        //        SimpleMappingTrace(items, TestCsvPath);
        //    }
        //    catch (TableMappingException ex)
        //    {
        //        Trace.WriteLine($"{ex.Message}; Row {ex.Row}");
        //        return;
        //    }
        //    Assert.Fail($"{nameof(TableMappingException)} has not been thrown");
        //}
    }
}