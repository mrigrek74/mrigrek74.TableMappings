using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using mrigrek74.TableMappings.Core.TableImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mrigrek74.TableMappings.Tests.TableImport.Csv
{
    /// <summary>
    /// Summary description for CsvTableImportTest
    /// </summary>
    [TestClass]
    public class CsvTableImportTest
    {
        public CsvTableImportTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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

        private const string TestCsvPath = "TableMapping/Csv/Test.csv";

        [TestMethod]
        public void ImportCsvFromPath()
        {
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(saver))
            {
                importer.Import(TestCsvPath);
            }
        }

        [TestMethod]
        public void ImportCsvFromPathAndEvents()
        {
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(saver, 15))
            {
                importer.Progress += Importer_Progress;
                importer.Import(TestCsvPath);
                importer.Progress -= Importer_Progress;
            }
        }

        private void Importer_Progress(object sender, DocumentImportEventArgs e)
        {
            Trace.WriteLine($"Imported {e.Rows} records");
        }

        [TestMethod]
        public async Task ImportCsvFromPathAsync()
        {
            var tokenSource = new CancellationTokenSource(60000);
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(saver))
            {
                await importer.ImportAsync(TestCsvPath, tokenSource.Token);
            }
        }

        [TestMethod]
        public async Task ImportCsvFromPathAsyncAndEvents()
        {
            var tokenSource = new CancellationTokenSource(60000);
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(saver, 15))
            {
                importer.Progress += Importer_Progress;
                await importer.ImportAsync(TestCsvPath, tokenSource.Token);
                importer.Progress -= Importer_Progress;
            }
        }
    }
}
