using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace mrigrek74.TableMappings.Core.TableMapping
{
    public abstract class TableMapperBase<T> : ITableMapper<T>
    {
        protected MappingOptions MappingOptions;

        protected readonly RowMapperBase<T> RowMapper;

        protected TableMapperBase(MappingOptions mappingOptions)
        {
            MappingOptions = mappingOptions ?? throw new ArgumentException(nameof(mappingOptions));

            switch (mappingOptions.MappingMode)
            {
                case MappingMode.ByName:
                    RowMapper = new ColumnNamesRowMapper<T>();
                    break;
                case MappingMode.ByNumber:
                    RowMapper = new ColumnNumbersRowMapper<T>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(mappingOptions.MappingMode), mappingOptions.MappingMode, null);
            }

            TypeDescriptorHelper<T>.AddProviderTransparent();
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

                var r = row;
                throw new TableMappingException($"{Strings.Row}: {r}; {aggregatedErrors}", r);
            }
        }

        protected void ThrowIfRowsLimitEnabled(int row, int indexRow)
        {
            if (MappingOptions.RowsLimit.HasValue && row > MappingOptions.RowsLimit)
            {
                throw new TableMappingException(
                    $"{Strings.ImportIsLimitedTo} {MappingOptions.RowsLimit} {Strings.Records}", indexRow);
            }
        }

        public abstract IList<T> Map(string path);
        public abstract IList<T> Map(Stream stream);
    }
}