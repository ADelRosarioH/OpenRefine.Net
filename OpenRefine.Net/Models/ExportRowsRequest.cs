using OpenRefine.Net.Helpers;

namespace OpenRefine.Net.Models
{
    public class ExportRowsRequest : BaseRequest
    {
        public string Format { get; set; } = ExportFormats.TSV; 
        public string Engine { get; set; } = "{\"facets\":[],\"mode\":\"row-based\"}";
        public string FileName { get; set; }
    }
}
