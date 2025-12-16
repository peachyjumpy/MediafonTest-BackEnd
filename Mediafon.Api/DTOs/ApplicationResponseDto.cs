namespace Mediafon.Api.DTOs
{
    public class ApplicationResponseDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
