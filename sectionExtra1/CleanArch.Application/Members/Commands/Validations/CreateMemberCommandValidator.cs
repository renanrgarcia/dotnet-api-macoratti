using System;
using FluentValidation;

namespace CleanArch.Application.Members.Commands.Validations;

public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(4, 100).WithMessage("First name must be between 4 and 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(4, 100).WithMessage("Last name must be between 4 and 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .MinimumLength(4).WithMessage("The gender must be a valid information");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("Active status is required");
    }
}
