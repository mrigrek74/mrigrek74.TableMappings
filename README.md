# Велосипед на тему маппинга/импорта/экспорта csv/xlsx таблиц.

```c#
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
