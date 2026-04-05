namespace TaskManagementApp.Services.Responses
{
    public class BaseServiceResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public BaseServiceResponse(bool success, string? message = null)
        {
            Success = success;
            Message = message;
        }
    }
}
