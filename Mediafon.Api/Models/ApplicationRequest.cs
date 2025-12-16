using System.ComponentModel.DataAnnotations;

namespace Mediafon.Api.Models
{
    public class ApplicationRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }        
        public User User { get; set; }
        [MaxLength(20)]
        public string Type { get; set; }
        [MaxLength(250)]
        public string Message { get; set; }
        [MaxLength(20)]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
