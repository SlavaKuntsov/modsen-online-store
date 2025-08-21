using FluentValidation;
using Utilities.Validators;

namespace OnlineStore.Application.Orders;

public class PlaceOrderCommandValidator : BaseCommandValidator<PlaceOrderCommand>
{
	public PlaceOrderCommandValidator()
	{
		RuleFor(x => x.ShippingAddress)
				.NotEmpty()
				.MaximumLength(200);

		RuleFor(x => x.DeliveryMethod)
				.IsInEnum();
	}
}