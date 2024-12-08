using FluentValidation;

namespace Modsen.DTO
{
public class EventDtoValidator : AbstractValidator<EventDto>
{
    public EventDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Event name is required.")
            .MaximumLength(100).WithMessage("Event name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.DateOfEvent)
            .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Event date must be in the future.");

        RuleFor(x => x.EventLocation)
            .NotEmpty().WithMessage("Event location is required.")
            .MaximumLength(100).WithMessage("Event location cannot exceed 100 characters.");

        RuleFor(x => x.EventCategory)
            .NotEmpty().WithMessage("Event category is required.")
            .MaximumLength(50).WithMessage("Event category cannot exceed 50 characters.");

        RuleFor(x => x.MaxMember)
            .GreaterThan(0).WithMessage("MaxMember must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("MaxMember cannot exceed 1000.");
    }
}
}