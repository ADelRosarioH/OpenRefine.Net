namespace OpenRefine.Net.Models
{
    public abstract class BaseRequest
    {
        public string Token { get; set; }
        public string ProjectId { get; set; }
    }
}
