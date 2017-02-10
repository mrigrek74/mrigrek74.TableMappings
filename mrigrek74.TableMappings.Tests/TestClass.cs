using mrigrek74.TableMappings.Core;

namespace mrigrek74.TableMappings.Tests
{
    public class TestClass
    {
        [ColumnName("Test String")]
        public string TestString { get; set; }
        [ColumnName("Test Int")]
        public int TestInt { get; set; }
    }
}
