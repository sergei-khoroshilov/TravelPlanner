using FluentValidation.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using TravelPlanner.Web.Validation;

namespace TravelPlanner.Web.Models
{
    [DataContract]
    [Validator(typeof(TripValidator))]
    public class Trip
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "destination")]
        [Required(ErrorMessage = "Destination is required")]
        [StringLength(128, ErrorMessage = "Destination length must be less then 128")]
        public string Destination { get; set; }

        [DataMember(Name = "startDate")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [DataMember(Name = "endDate")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [DataMember(Name = "comment")]
        [StringLength(256, ErrorMessage = "Comment length must be less then 256")]
        public string Comment { get; set; }
    }
}