﻿using System.Collections.Generic;
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
    public class CsvTableMappingByColNumbersTest
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
       

        private void SimpleMappingTrace(ICollection<TestClass2> items, string path)
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
            var mapper = new CsvMapper<TestClass2>(MappingMode.ByNumber);
            var items = mapper.Map(TestCsvPath);
            Assert.IsNotNull(items, "Result is null");
            Assert.IsTrue(items.Any(), "Items empty");

            SimpleMappingTrace(items , TestCsvPath);
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