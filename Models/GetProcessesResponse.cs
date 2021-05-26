using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenRefine.Net.Models
{
    public class GetProcessesResponse : BaseResponse
    {
        public IReadOnlyCollection<JObject> Processes { get; set; }
    }
}
