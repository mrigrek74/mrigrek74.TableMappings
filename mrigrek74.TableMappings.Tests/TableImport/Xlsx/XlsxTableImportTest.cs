using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using mrigrek74.TableMappings.Core;
using mrigrek74.TableMappings.Core.Epplus.TableImport;
using mrigrek74.TableMappings.Core.TableImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mrigrek74.TableMappings.Tests.TableImport.Xlsx
{
    [TestClass]
    public class XlsxTableImportTest
    {
        private const string TestXlsxPath = "TableMapping/Xlsx/Test.xlsx";

        [TestMethod]
        public void ImportCsvFromPath()
        {
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                importer.Import(TestXlsxPath);
            }
        }

        [TestMethod]
        public void ImportXlsxFromPathAndEvents()
        {
            var saver = new TestInsertSaver();
            using (var importer = new XlsxTableImporter<TestClass>(new MappingOptions(), saver))
            {
                saver.Progress += Importer_Progress;
                importer.Import(TestXlsxPath);
                saver.Progress -= Importer_Progress;
            }
        }

        private void Importer_Progress(object sender, DocumentImportEventArgs e)
        {
            Trace.WriteLine($"Imported {e.Rows} records");
            Trace.WriteLine("");
        }

        [TestMethod]
        public async Task ImportXlsxFromPathAsync()
        {
            var tokenSource = new CancellationTokenSource(60_000);
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestXlsxPath, tokenSource.Token);
            }
        }

        [TestMethod]
        public async Task ImportXlsxFromPathAsyncAndEvents()
        {
            var tokenSource = new CancellationTokenSource(60_000);
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                saver.Progress += Importer_Progress;
                await importer.ImportAsync(TestXlsxPath, tokenSource.Token);
                saver.Progress -= Importer_Progress;
            }
        }
    }
}