using System.ComponentModel;

namespace OnlineStore.Domain.Enums;

public enum DeliveryMethod
{
	[Description(nameof(Standard))]
	Standard = 0,
	[Description(nameof(Express))]
	Express = 1
}