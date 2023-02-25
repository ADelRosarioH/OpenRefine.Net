namespace OpenRefine.Net.Models
{
    public abstract class BaseRequest
    {
        public string CsrfToken { get; set; }
        public string ProjectId { get; set; }
    }
}
