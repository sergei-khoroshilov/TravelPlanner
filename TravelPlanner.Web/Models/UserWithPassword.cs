using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TravelPlanner.Web.Models
{
    public class UserWithPassword : User
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters")]
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
