Велосипед на тему маппинга/импорта/экспорта csv/xlsx таблиц.

```c#
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
```

## Маппинг

```c#
           var mapper = new CsvMapper<TestClass>(new MappingOptions
            {
                MappingMode = MappingMode.ByName,
                HasHeader = true,
                SuppressConvertTypeErrors = true,
                EnableValidation = true,
                RowsLimit = 1_000,
                Trim = true
            });
            var items = mapper.Map(CsvPath);
```

## Экспорт
```c#
            var exporter = new CsvTableExporter<TestClass>();
            exporter.Export(list, fname);
```

## Импорт
