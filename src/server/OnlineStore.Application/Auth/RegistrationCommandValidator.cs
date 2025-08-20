using System.Globalization;
using Domain.Constants;
using FluentValidation;
using Utilities.Validators;

namespace OnlineStore.Application.Auth;

public class RegistrationCommandValidator : BaseCommandValidator<UserRegistrationCommand>
{
	public RegistrationCommandValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.WithMessage("Email is required.")
			.EmailAddress()
			.WithMessage("Email is not a valid email address.");

		RuleFor(x => x.Password)
			.NotEmpty()
			.WithMessage("Password is required.")
			.MinimumLength(8)
			.WithMessage("Password must be at least 8 characters.")
			.Matches("[A-Z]")
			.WithMessage("Password must contain at least one uppercase letter.")
			.Matches("[a-z]")
			.WithMessage("Password must contain at least one lowercase letter.")
			.Matches(@"\d")
			.WithMessage("Password must contain at least one digit.")
			.Matches(@"[\W_]")
			.WithMessage("Password must contain at least one special character.");

		RuleFor(x => x.FirstName)
			.NotEmpty()
			.WithMessage("First name is required.")
			.MaximumLength(100)
			.WithMessage("First name is too long.");

		RuleFor(x => x.LastName)
			.NotEmpty()
			.WithMessage("Last name is required.")
			.MaximumLength(100)
			.WithMessage("Last name is too long.");

		RuleFor(x => x.DateOfBirth)
			.NotEmpty()
			.WithMessage("Date of birth is required.")
			.Must(BeAValidIsoDate)
			.WithMessage("DateOfBirth must be a valid date in yyyy-MM-dd format.")
			.Must(BeOlderThanMinimumAge)
			.WithMessage("User must be at least 13 years old.");
	}

	private static bool BeAValidIsoDate(string dob)
	{
		if (string.IsNullOrWhiteSpace(dob))
			return false;

		return DateTime.TryParseExact(
			dob,
			DateTimeConstants.DateFormat,
			CultureInfo.InvariantCulture,
			DateTimeStyles.None,
			out _);
	}

	private static bool BeOlderThanMinimumAge(string dob)
	{
		if (!DateTime.TryParseExact(
				dob,
				DateTimeConstants.DateFormat,
				CultureInfo.InvariantCulture,
				DateTimeStyles.None,
				out var date))
		{
			return false;
		}

		var today = DateTime.UtcNow.Date;
		var age = today.Year - date.Year;

		if (date > today.AddYears(-age))
			age--;

		return age >= 13;
	}
}