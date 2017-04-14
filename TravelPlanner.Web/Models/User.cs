using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using TravelPlanner.DB;

namespace TravelPlanner.Web.Models
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(128, ErrorMessage = "Name length must be less then 128")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be valid email address")]
        [StringLength(32, ErrorMessage = "Email length must be less then 32")]
        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "type")]
        public UserType Type { get; set; } = UserType.User;
    }
}