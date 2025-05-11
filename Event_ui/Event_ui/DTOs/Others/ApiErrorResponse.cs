namespace Event_ui.DTOs
{
    public class ProblemDetailsResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
        public string TraceId { get; set; }
    }
    public class ApiErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
