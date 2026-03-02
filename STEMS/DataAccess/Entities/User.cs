using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class User : BaseEntity
    {
        [Column(TypeName = "nvarchar(255)")]
        public string Username { get; set; }

        public string Password { get; set; }

        public string ProfileId { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual Profile Profile { get; set; }

        public string Status { get; set; } // Active, Deactive

        public User() { }

        public User(string id) : this()
        {
            Id = id;
        }
    }
}
