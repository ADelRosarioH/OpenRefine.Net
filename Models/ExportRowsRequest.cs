using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRefine.Net.Models
{
    public class ExportRowsRequest : BaseRequest
    {
        public string Format { get; set; } = ExportFormats.TSV; 
        public string Engine { get; set; } = "{\"facets\":[],\"mode\":\"row-based\"}";
        public string DownloadPath { get; set; }
    }
}
