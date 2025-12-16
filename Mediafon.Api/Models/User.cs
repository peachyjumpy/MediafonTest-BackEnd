using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mediafon.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [MaxLength(200)]
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<ApplicationRequest> ApplicationRequests { get; set; }
        = new List<ApplicationRequest>();


    }
}
