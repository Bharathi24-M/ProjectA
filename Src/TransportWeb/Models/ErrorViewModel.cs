namespace TransportWeb.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public int Errorcode { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}