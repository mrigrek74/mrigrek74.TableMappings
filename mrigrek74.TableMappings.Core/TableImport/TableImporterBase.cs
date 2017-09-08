﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mrigrek74.TableMappings.Core.TableImport
{
    public abstract class TableImporterBase<T> : ITableImporter, IDisposable
    {
        protected bool EnableValidation;
        protected bool SuppressConvertTypeErrors;
        protected int? RowsLimit;
        private int? _eventInterval;

        public event EventHandler<DocumentImportEventArgs> Progress;
        protected virtual void OnProgress(DocumentImportEventArgs e)
        {
            if (_eventInterval.HasValue && e.Rows % _eventInterval.Value == 0)
            {
                Progress?.Invoke(this, e);
            }
        }

        protected readonly RowMapperBase<T> RowMapper;
        protected IRowSaver<T> RowSaver;

        private void TypeDescriptorAddProviderTransparent()
        {
            var metadataType = typeof(T)
                .GetCustomAttributes(typeof(MetadataTypeAttribute), true)
                .OfType<MetadataTypeAttribute>()
                .FirstOrDefault();

            if (metadataType != null)
            {
                TypeDescriptor.AddProviderTransparent(
                    new AssociatedMetadataTypeTypeDescriptionProvider(typeof(T),
                    metadataType.MetadataClassType), typeof(T));
            }
        }

        protected TableImporterBase(MappingMode mappingMode, IRowSaver<T> rowSaver)
        {
            EnableValidation = false;
            SuppressConvertTypeErrors = true;
            RowsLimit = null;
            RowSaver = rowSaver;

            switch (mappingMode)
            {
                case MappingMode.ByName:
                    RowMapper = new ColumnNamesRowMapper<T>();
                    break;
                //case MappingMode.ByNumber:
                //    RowMapper = new ColumnNumbersRowMapper<T>();
                //    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mappingMode), mappingMode, null);
            }

            TypeDescriptorAddProviderTransparent();
        }

        protected TableImporterBase(MappingMode mappingMode, IRowSaver<T> rowSaver,
            int? eventInterval) : this(mappingMode, rowSaver)
        {
            EnableValidation = false;
            SuppressConvertTypeErrors = true;
            RowsLimit = null;
            _eventInterval = eventInterval;

            TypeDescriptorAddProviderTransparent();
        }

        protected TableImporterBase(
            MappingMode mappingMode,
            IRowSaver<T> rowSaver,
            int? eventInterval,
            bool enableValidation): this(mappingMode, rowSaver)
        {
            EnableValidation = enableValidation;
            SuppressConvertTypeErrors = true;
            RowsLimit = null;
            RowSaver = rowSaver;
            _eventInterval = eventInterval;

            TypeDescriptorAddProviderTransparent();
        }


        protected TableImporterBase(
            MappingMode mappingMode,
            IRowSaver<T> rowSaver,
            int? eventInterval,
            bool enableValidation, bool suppressConvertTypeErrors, int? rowsLimit)
            : this(mappingMode, rowSaver)
        {
            EnableValidation = enableValidation;
            SuppressConvertTypeErrors = suppressConvertTypeErrors;
            RowsLimit = rowsLimit;
            _eventInterval = eventInterval;

            TypeDescriptorAddProviderTransparent();
        }


        protected void ValidateRow(T entity, int row)
        {
            if (!EnableValidation)
                return;

            var context = new ValidationContext(entity, null, null);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, context, validationResults, true))
            {
                var aggregatedErrors = validationResults
                    .Select(x => x.ErrorMessage)
                    .Aggregate((item, next) => next + "; " + item);

                throw new TableMappingException($"{Strings.Row}: {row + 1}; {aggregatedErrors}", row);
            }
        }

        protected void ThrowIfRowsLimitEnabled(int row)
        {
            if (RowsLimit.HasValue && row > RowsLimit)
            {
                throw new TableMappingException(
                    $"{Strings.ImportIsLimitedTo} {RowsLimit} {Strings.Records}", row);
            }
        }


        public abstract void Import(string path);

        public abstract void Import(Stream stream);

        public abstract Task ImportAsync(string path, CancellationToken cancellationToken);

        public abstract Task ImportAsync(Stream stream, CancellationToken cancellationToken);

        public void Dispose()
        {
            RowSaver.Dispose();
        }
    }
}