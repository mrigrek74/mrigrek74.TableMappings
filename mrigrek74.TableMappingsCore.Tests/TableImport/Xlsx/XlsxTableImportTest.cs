using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using mrigrek74.TableMappingsCore.Core;
using mrigrek74.TableMappingsCore.Core.Epplus.TableImport;
using mrigrek74.TableMappingsCore.Core.TableImport;
using Xunit;
using Xunit.Abstractions;

namespace mrigrek74.TableMappingsCore.Tests.TableImport.Xlsx
{
    
    public class XlsxTableImportTest
    {
        private const string TestXlsxPath = "TableMapping/Xlsx/Test.xlsx";
        private readonly ITestOutputHelper _output;

        public XlsxTableImportTest(ITestOutputHelper output)
        {
            _output = output;
            var culture = new CultureInfo(TestConstants.Culture);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
        [Fact]
        public void ImportCsvFromPath()
        {
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                importer.Import(TestXlsxPath);
            }
        }

        [Fact]
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
            _output.WriteLine($"Imported {e.Rows} records");
            _output.WriteLine("");
        }

        [Fact]
        public async Task ImportXlsxFromPathAsync()
        {
            var tokenSource = new CancellationTokenSource(60_000);
            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestXlsxPath, tokenSource.Token);
            }
        }

        [Fact]
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