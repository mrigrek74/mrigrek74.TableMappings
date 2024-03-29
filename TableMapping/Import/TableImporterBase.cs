﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TableMapping.Import
{
    public abstract class TableImporterBase<T> : ITableImporter, IDisposable
    {
        protected readonly MappingOptions MappingOptions;

        protected readonly RowMapperBase<T> RowMapper;
        protected IRowSaver<T> RowSaver;

        protected TableImporterBase(MappingOptions mappingOptions, IRowSaver<T> rowSaver)
        {
            MappingOptions = mappingOptions ?? throw new ArgumentException(nameof(mappingOptions));
            RowSaver = rowSaver;

            switch (MappingOptions.MappingMode)
            {
                case MappingMode.ByName:
                    RowMapper = new ColumnNamesRowMapper<T>();
                    break;
                case MappingMode.ByNumber:
                    RowMapper = new ColumnNumbersRowMapper<T>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(MappingOptions.MappingMode), MappingOptions.MappingMode, null
                    );
            }

        }

        protected void ValidateRow(T entity, int row)
        {
            if (!MappingOptions.EnableValidation)
                return;

            var context = new ValidationContext(entity, null, null);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, context, validationResults, true))
            {
                var aggregatedErrors = validationResults
                    .Select(x => x.ErrorMessage)
                    .Aggregate((item, next) => next + "; " + item);

                throw new TableMappingException($"{Strings.Row}: {row}; {aggregatedErrors}", row);
            }
        }

        protected void ThrowIfRowsLimitEnabled(int row)
        {
            if (MappingOptions.RowsLimit.HasValue && row > MappingOptions.RowsLimit)
            {
                throw new TableMappingException(
                    $"{Strings.ImportIsLimitedTo} {MappingOptions.RowsLimit} {Strings.Records}", row
                );
            }
        }

        public abstract Task ImportAsync(string path, CancellationToken cancellationToken);

        public abstract Task ImportAsync(Stream stream, CancellationToken cancellationToken);

        public void Dispose()
        {
            RowSaver.Dispose();
        }
    }
}