namespace OpenRefine.Net.Models
{
    public class CreateProjectRequest : BaseRequest
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string ProjectName { get; set; }
        public string Format { get; set; }
        public object Options { get; set; }
    }
}
