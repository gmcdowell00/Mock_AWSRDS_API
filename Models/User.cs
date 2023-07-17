using System.ComponentModel.DataAnnotations;

namespace Mock_AWSRDS_API.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }        
        public string HashPassword { get; set; }

        public string SaltString { get; set; }
    }
}
