using System.Collections.Generic;
using System.Text.Json;

namespace OpenRefine.Net.Models
{
    public class GetProcessesResponse : BaseResponse
    {
        public IReadOnlyCollection<JsonElement> Processes { get; set; }
    }
}
