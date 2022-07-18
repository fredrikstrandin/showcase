namespace CommonLib.Models
{
    public class ErrorItem
    {
        public ErrorItem(string message, int httpStatusCode)
        {
            Message = message;
            HttpStatusCode = httpStatusCode;
        }
        public string Message { get; }
        public int HttpStatusCode { get; }

    }
}
