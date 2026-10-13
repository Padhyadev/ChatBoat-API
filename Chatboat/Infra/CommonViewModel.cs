namespace Chatboat.Infra
{
    public class CommonViewModel
    {
        public bool IsSuccess { get; set; } = false;
        public int StatusCode { get; set; } = ResponseStatusCode.Error;
        public string? Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

    }
}
