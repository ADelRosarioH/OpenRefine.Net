using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRefine.Net.Models
{
    public class CreateProjectRequest
    {
        public byte[] FileContent { get; set; }
        public string ProjectName { get; set; }
        public string Format { get; set; }
        public object Options { get; set; }
    }
}
