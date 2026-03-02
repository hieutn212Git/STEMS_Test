using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Profile")]
    public class Profile : BaseEntity
    {
        public string Email { get; set; }

        public DateTime Dob { get; set; }

        public string PhoneNumber { get; set; }

        public string Grade { get; set; }

        public Profile()
        {
        }

        public Profile(string id) : this()
        {
            Id = id;
        }
    }
}