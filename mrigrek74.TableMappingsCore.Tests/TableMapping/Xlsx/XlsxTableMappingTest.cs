﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using mrigrek74.TableMappingsCore.Core;
using mrigrek74.TableMappingsCore.Core.Epplus.TableMapping;
using Xunit;
using Xunit.Abstractions;

namespace mrigrek74.TableMappingsCore.Tests.TableMapping.Xlsx
{

    public class XlsxTableMappingTest
    {
        private const string TestXlsxPath = "TableMapping/Xlsx/Test.xlsx";
        private const string ValidationTestXlsxPath = "TableMapping/Xlsx/ValidationTest.xlsx";
        private const string SuppressConvertTypeErrorsTestXlsxPath
            = "TableMapping/Xlsx/SuppressConvertTypeErrorsTest.xlsx";

        private readonly ITestOutputHelper _output;
        public XlsxTableMappingTest(ITestOutputHelper output)
        {
            _output = output;
            var culture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        [Fact]
        public void SimpleMappingByPath()
        {
            var mapper = new XlsxMapper<TestClass>(new MappingOptions { });
            var items = mapper.Map(TestXlsxPath);
            Assert.NotNull(items);
            Assert.True(items.Any(), "items empty");

            items.SimpleMappingTrace(TestXlsxPath, _output);
        }

        [Fact]
        public void SimpleMappingByStream()
        {
            var mapper = new XlsxMapper<TestClass>(new MappingOptions { });
            using (var fs = new FileStream(TestXlsxPath, FileMode.Open))
            {
                var items = mapper.Map(fs);

                Assert.NotNull(items);
                Assert.True(items.Any(), "items empty");

                items.SimpleMappingTrace(TestXlsxPath, _output);
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
            var mapper = new XlsxMapper<ValidationTestClass>(new MappingOptions { EnableValidation = true });

            try
            {
                var items = mapper.Map(ValidationTestXlsxPath);
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
                var mapper = new XlsxMapper<TestClass>(new MappingOptions { RowsLimit = 99 });
                var items = mapper.Map(TestXlsxPath);
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
            var mapper = new XlsxMapper<TestClass>(new MappingOptions
            {
                HasHeader = true,
                RowsLimit = 100
            });
            var items = mapper.Map(TestXlsxPath);
            Assert.NotNull(items);
            Assert.True(items.Any(), "items empty");

            items.SimpleMappingTrace(TestXlsxPath, _output);
        }

        [Fact]
        public void MappingWithSuppressConvertTypeErrors()
        {
            try
            {
                var mapper = new XlsxMapper<TestClass>(new MappingOptions { SuppressConvertTypeErrors = false });
                var items = mapper.Map(SuppressConvertTypeErrorsTestXlsxPath);
                Assert.NotNull(items);
                Assert.True(items.Any(), "items empty");

                items.SimpleMappingTrace(TestXlsxPath, _output);
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