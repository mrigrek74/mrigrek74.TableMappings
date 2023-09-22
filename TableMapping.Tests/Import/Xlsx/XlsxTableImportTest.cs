using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TableMapping.EPPlus.Import;
using TableMapping.Import;
using Xunit;
using Xunit.Abstractions;

namespace TableMapping.Tests.Import.Xlsx
{
    
    public class XlsxTableImportTest
    {
        private const string TestXlsxPath = "Mapping/Xlsx/Test.xlsx";
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
        public async Task ImportCsvFromPath()
        {
            var tokenSource = new CancellationTokenSource(60_000);

            var saver = new TestInsertSaver();
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestXlsxPath, tokenSource.Token);
            }
        }

        [Fact]
        public async Task ImportXlsxFromPathAndEvents()
        {
            var tokenSource = new CancellationTokenSource(60_000);

            var saver = new TestInsertSaver()
            {
                Log = (msg) => _output.WriteLine(msg)
            };
            using (var importer = new XlsxTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestXlsxPath, tokenSource.Token);
            }
        }
         

        [Fact]
        public async Task ImportXlsxFromPathAsync()
        {
            var tokenSource = new CancellationTokenSource(60_000);
            var saver = new TestInsertSaver()
            {
                Log = (msg) => _output.WriteLine(msg)
            };
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestXlsxPath, tokenSource.Token);
            }
        }

        [Fact]
        public async Task ImportXlsxFromPathAsyncAndEvents()
        {
            var tokenSource = new CancellationTokenSource(60_000);
            var saver = new TestInsertSaver() 
            {
                Log = (msg) => _output.WriteLine(msg) 
            };
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestXlsxPath, tokenSource.Token);
            }
        }
    }
}