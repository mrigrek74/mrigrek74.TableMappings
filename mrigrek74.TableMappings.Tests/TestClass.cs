using System;
using mrigrek74.TableMappings.Core;

namespace mrigrek74.TableMappings.Tests
{
    public class TestClass
    {
        [ColumnName("Test String")]
        public string TestString { get; set; }

        [ColumnName("Test Int")]
        public int TestInt { get; set; }
        [ColumnName("Test Int?")]
        public int? TestIntNullable { get; set; }

        [ColumnName("Test Float")]
        public float TestFloat { get; set; }
        [ColumnName("Test Float?")]
        public float? TestFloatNullable { get; set; }

        [ColumnName("Test Decimal")]
        public decimal TestDecimal { get; set; }
        [ColumnName("Test Decimal?")]
        public decimal? TestDecimalNullable { get; set; }

        [ColumnName("Test Double")]
        public decimal TestDouble { get; set; }
        [ColumnName("Test Double?")]
        public decimal? TestDoubleNullable { get; set; }

        [ColumnName("Test Guid")]
        public Guid TestGuid { get; set; }
        [ColumnName("Test Guid?")]
        public Guid? TestGuidNullable { get; set; }

        [ColumnName("Test DateTime")]
        public DateTime TestDateTime { get; set; }
        [ColumnName("Test DateTime?")]
        public DateTime? TestDateTimeNullable { get; set; }
    }
}
