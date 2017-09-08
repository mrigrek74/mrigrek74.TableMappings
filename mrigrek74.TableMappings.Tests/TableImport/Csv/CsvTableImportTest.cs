using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using mrigrek74.TableMappings.Core;
using mrigrek74.TableMappings.Core.TableImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mrigrek74.TableMappings.Tests.TableImport.Csv
{
    [TestClass]
    public class CsvTableImportTest
    {
        private const string TestCsvPath = "TableMapping/Csv/Test.csv";

        [TestMethod]
        public void ImportCsvFromPath()
        {
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(MappingMode.ByName, saver))
            {
                importer.Import(TestCsvPath);
            }
        }

        [TestMethod]
        public void ImportCsvFromPathAndEvents()
        {
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(MappingMode.ByName, saver, 15))
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
            using (var importer = new CsvTableImporter<TestClass>(MappingMode.ByName, saver))
            {
                await importer.ImportAsync(TestCsvPath, tokenSource.Token);
            }
        }

        [TestMethod]
        public async Task ImportCsvFromPathAsyncAndEvents()
        {
            var tokenSource = new CancellationTokenSource(60000);
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(MappingMode.ByName, saver, 15))
            {
                importer.Progress += Importer_Progress;
                await importer.ImportAsync(TestCsvPath, tokenSource.Token);
                importer.Progress -= Importer_Progress;
            }
        }
    }
}
