namespace mrigrek74.TableMappingsCore.Core
{
    public enum MappingMode
    {
        ByName,
        ByNumber
    }

    public class MappingOptions
    {
        public MappingMode MappingMode { get; set; } = MappingMode.ByName;
        public bool EnableValidation { get; set; }
        public bool SuppressConvertTypeErrors { get; set; } = true;
        public int? RowsLimit { get; set; }
        public bool Trim { get; set; } = true;
        private bool _hasHeader;

        public bool HasHeader
        {
            get => MappingMode == MappingMode.ByName || _hasHeader;
            set => _hasHeader = value;
        }
    }
}