using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.DB
{
    [Table("Users")]
    public class User
    {
        [Column("Id"), Key]
        public long Id { get; set; }

        [Column("Name"), Required]
        public string Name { get; set; }

        [Column("Email"), Required]
        public string Email { get; set; }

        [Column("Password"), Required]
        public string Password { get; set; }

        [Column("Type"), Required]
        public UserType Type { get; set; } = UserType.User;
    }
}
