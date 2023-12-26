namespace LocatedAPI.Models
{
    public class ApiResp
    {
        public string Token { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResp()
        {
        }

        public ApiResp(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ApiResp(string token, int statusCode, string message)
        {
            Token = token;
            StatusCode = statusCode;
            Message = message;
        }
    }
}
