using Domain.Exceptions;

namespace Domain.Entities.OrderAggregate.Exceptions;

internal class ChangeOrderProductNotAbleException() :
	DomainException(DefaultChangeOrderProductNotAbleMessageTemplate)
{
	private const string DefaultChangeOrderProductNotAbleMessageTemplate = "Is not possiblea changer product when order is not Creating";
}
