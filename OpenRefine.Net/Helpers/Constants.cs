namespace OpenRefine.Net.Helpers
{
    public static class SupportedFormats
    {
        public static readonly string TextLineBased = "text/line-based";
        public static readonly string TextLineBasedSeparatorBased = "text/line-based/*sv";
        public static readonly string TextLineBasedFixedWidth = "text/line-based/fixed-width";
        public static readonly string BinaryTextXmlXlsXlsx = "binary/text/xml/xls/xlsx";
        public static readonly string TextJson = "text/json";
        public static readonly string TextXml = "text/xml";
    }

    public static class ExportFormats
    {
        public static readonly string TSV = "TSV";
        public static readonly string CSV = "CSV";
        public static readonly string XLS = "XLS";
        public static readonly string XLSX = "XLSX";
        public static readonly string ODS = "ODS";
        public static readonly string HTML = "HTML";
    }
}
