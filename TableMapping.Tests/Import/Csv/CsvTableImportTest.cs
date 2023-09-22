using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TableMapping.Import;
using Xunit;
using Xunit.Abstractions;

namespace TableMapping.Tests.Import.Csv
{
    public class CsvTableImportTest
    {
        private const string TestCsvPath = "Mapping/Csv/Test.csv";
        private readonly ITestOutputHelper _output;
        public CsvTableImportTest(ITestOutputHelper output)
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

            var saver = new TestInsertSaver() {
                Log = (msg) => _output.WriteLine(msg)
            };
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestCsvPath, tokenSource.Token);
            }
        }

        [Fact]
        public async Task ImportCsvFromPathAndEvents()
        {
            var tokenSource = new CancellationTokenSource(60_000);
            var saver = new TestInsertSaver()
            {
                Log = (msg) => _output.WriteLine(msg)
            };
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestCsvPath, tokenSource.Token);
            }
        }

   
        [Fact]
        public async Task ImportCsvFromPathAsync()
        {
            var tokenSource = new CancellationTokenSource(60_000);
            var saver = new TestInsertSaver()
            {
                Log = (msg) => _output.WriteLine(msg)
            };
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestCsvPath, tokenSource.Token);
            }
        }

        [Fact]
        public async Task ImportCsvFromPathAsyncAndEvents()
        {
            var tokenSource = new CancellationTokenSource(60_000);
            var saver = new TestInsertSaver()
            {
                Log = (msg) => _output.WriteLine(msg)
            };
            using (var importer = new CsvTableImporter<TestClass>(new MappingOptions(), saver))
            {
                await importer.ImportAsync(TestCsvPath, tokenSource.Token);                
            }
        }
    }
}