using System.Collections.Generic;

namespace OpenRefine.Net.Models
{
    public abstract class BaseResponse
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public ICollection<Dictionary<string, string>> Results { get; set; }
    }
}
