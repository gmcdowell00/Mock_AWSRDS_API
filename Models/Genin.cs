using Amazon.S3.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mock_AWSRDS_API.Models
{
    public class Genin
    {
        [Key]
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }

        [ForeignKey(name:"Id")]
        public Guid JoninId { get; set; }

        public virtual Jonin Jonin { get; set; }
    }
}
