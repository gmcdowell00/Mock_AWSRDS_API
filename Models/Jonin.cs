using System.ComponentModel.DataAnnotations;

namespace Mock_AWSRDS_API.Models
{
    public class Jonin
    {
        [Key]
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }        

        public virtual ICollection<Genin> Genin { get; set; }

    }
}
