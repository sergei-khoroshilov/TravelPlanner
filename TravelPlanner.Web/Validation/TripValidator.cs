using FluentValidation;
using TravelPlanner.Web.Models;

namespace TravelPlanner.Web.Validation
{
    public class TripValidator : AbstractValidator<Trip>
    {
        public TripValidator()
        {
            RuleFor(t => t.EndDate).GreaterThanOrEqualTo(t => t.StartDate)
                                   .WithMessage("End date must not be earlier than start date");
        }
    }
}
