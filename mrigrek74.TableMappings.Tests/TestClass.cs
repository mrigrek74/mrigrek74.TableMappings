using System;
using mrigrek74.TableMappings.Core;

namespace mrigrek74.TableMappings.Tests
{
    public class TestClass
    {
        [ColumnName("Test String")]
        [ColumnNumber(0)]
        public string TestString { get; set; }

        [ColumnName("Test Int")]
        [ColumnNumber(1)]
        public int TestInt { get; set; }

        [ColumnName("Test Int?")]
        [ColumnNumber(2)]
        public int? TestIntNullable { get; set; }

        [ColumnName("Test Float")]
        [ColumnNumber(3)]
        public float TestFloat { get; set; }

        [ColumnName("Test Float?")]
        [ColumnNumber(4)]
        public float? TestFloatNullable { get; set; }

        [ColumnName("Test Decimal")]
        [ColumnNumber(5)]
        public decimal TestDecimal { get; set; }

        [ColumnName("Test Decimal?")]
        [ColumnNumber(6)]
        public decimal? TestDecimalNullable { get; set; }

        [ColumnName("Test Double")]
        [ColumnNumber(7)]
        public decimal TestDouble { get; set; }

        [ColumnName("Test Double?")]
        [ColumnNumber(8)]
        public decimal? TestDoubleNullable { get; set; }

        [ColumnName("Test Guid")]
        [ColumnNumber(9)]
        public Guid TestGuid { get; set; }

        [ColumnName("Test Guid?")]
        [ColumnNumber(10)]
        public Guid? TestGuidNullable { get; set; }

        [ColumnName("Test DateTime")]
        [ColumnNumber(11)]
        public DateTime TestDateTime { get; set; }

        [ColumnName("Test DateTime?")]
        [ColumnNumber(12)]
        public DateTime? TestDateTimeNullable { get; set; }
    }

    public class TestClass2
    {
        //[ColumnNumber(12)]
        //public string TestString { get; set; }

        //[ColumnNumber(11)]
        //public int TestInt { get; set; }

        //[ColumnNumber(10)]
        //public int? TestIntNullable { get; set; }

        //[ColumnNumber(9)]
        //public float TestFloat { get; set; }

        //[ColumnNumber(8)]
        //public float? TestFloatNullable { get; set; }

        //[ColumnNumber(7)]
        //public decimal TestDecimal { get; set; }

        //[ColumnNumber(6)]
        //public decimal? TestDecimalNullable { get; set; }

        //[ColumnNumber(5)]
        //public decimal TestDouble { get; set; }

        //[ColumnNumber(4)]
        //public decimal? TestDoubleNullable { get; set; }

        //[ColumnNumber(3)]
        //public Guid TestGuid { get; set; }





        [ColumnNumber(12)]
        public DateTime? TestDateTimeNullable { get; set; }

        [ColumnNumber(11)]
        public DateTime TestDateTime { get; set; }

        [ColumnNumber(10)]
        public Guid? TestGuidNullable { get; set; }
    }
}