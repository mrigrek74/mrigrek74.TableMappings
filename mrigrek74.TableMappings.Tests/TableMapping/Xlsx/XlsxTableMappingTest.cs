using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using mrigrek74.TableMappings.Core;
using mrigrek74.TableMappings.Core.Epplus.TableMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mrigrek74.TableMappings.Tests.TableMapping.Xlsx
{
    /// <summary>
    /// Summary description for XlsxTableMapping
    /// </summary>
    [TestClass]
    public class XlsxTableMappingTest
    {
        public XlsxTableMappingTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        private const string TestXlsxPath = "TableMapping/Xlsx/TestXlsx.xlsx";

        private void SimpleMappingTrace(ICollection<TestClass> items)
        {
            Trace.WriteLine(TestXlsxPath);
            Trace.WriteLine("");
            const int take = 100;
            Trace.WriteLine($"First {take} of {items.Count}:");

            foreach (var item in items.Take(take))
            {
                foreach (var p in item.GetType().GetProperties())
                {
                    Trace.WriteLine($"{p.Name};{p.GetValue(item, null)}");
                }
            }
        }


        [TestMethod]
        public void SimpleMappingByPath()
        {
            var mapper = new XlsxMapper<TestClass>();
            var items = mapper.Map(TestXlsxPath);
            Assert.IsNotNull(items, "Result is null");
            Assert.IsTrue(items.Any(), "items empty");

            SimpleMappingTrace(items);
        }

        [TestMethod]
        public void SimpleMappingByStream()
        {
            var mapper = new XlsxMapper<TestClass>();
            using (var fs = new FileStream(TestXlsxPath, FileMode.Open))
            {
                var items = mapper.Map(fs);

                Assert.IsNotNull(items, "Result is null");
                Assert.IsTrue(items.Any(), "items empty");

                SimpleMappingTrace(items);
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
        private const string ValidationTestXlsxPath = "TableMapping/Xlsx/ValidationTestXlsx.xlsx";


        [TestMethod]
        public void MappingWithValidation()
        {
            var mapper = new XlsxMapper<ValidationTestClass>(true);

            try
            {
                var items = mapper.Map(ValidationTestXlsxPath);
            }
            catch (ValidationException vex)
            {
                Trace.WriteLine(vex.Message);
                return;
            }

            Assert.Fail("Validation Exception has not been thrown");
        }


        [TestMethod]
        public void MappingWithRowLimit()
        {
            try
            {
                var mapper = new XlsxMapper<TestClass>(false, true, 99);
                var items = mapper.Map(TestXlsxPath);
            }
            catch (InvalidOperationException ioex)
            {
                Trace.WriteLine(ioex.Message);
                return;
            }
            Assert.Fail("InvalidOperationException has not been thrown");
        }

        [TestMethod]
        public void MappingWithRowLimit2()
        {
            var mapper = new XlsxMapper<TestClass>(false, true, 100);
            var items = mapper.Map(TestXlsxPath);
            Assert.IsNotNull(items, "Result is null");
            Assert.IsTrue(items.Any(), "items empty");

            SimpleMappingTrace(items);
        }


        private const string SuppressConvertTypeErrorsTestXlsxPath
            = "TableMapping/Xlsx/SuppressConvertTypeErrorsTestXlsx.xlsx";

        [TestMethod]
        public void MappingWithSuppressConvertTypeErrors()
        {
            try
            {
                var mapper = new XlsxMapper<TestClass>(false, false, null);
                var items = mapper.Map(SuppressConvertTypeErrorsTestXlsxPath);
                Assert.IsNotNull(items, "Result is null");
                Assert.IsTrue(items.Any(), "items empty");

                SimpleMappingTrace(items);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return;
            }
            Assert.Fail("Exception has not been thrown");
        }

    }
}