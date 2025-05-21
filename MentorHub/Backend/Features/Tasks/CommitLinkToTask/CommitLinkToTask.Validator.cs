using FluentValidation;

namespace Backend.Features.Tasks.CommitLinkToTask
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0)
                .WithMessage("Task ID must be greater than zero.");

            RuleFor(x => x.CommitUrl)
                .NotEmpty()
                .WithMessage("Commit URL is required.")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out var parsed)
                             && (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps))
                .WithMessage("Commit URL must be a valid absolute URL.");
        }
    }
}
