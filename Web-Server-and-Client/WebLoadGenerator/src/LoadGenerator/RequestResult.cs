using System.Net;

namespace msantana.amaral.LoadGenerator
{
    public class RequestResult
    {
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
    }
}
