using System.Collections;

namespace Application.Dtos.OrderResponse;

public record OrderUpdateOrderProductResponse
{
	public int OrderId { get; init; }
	public decimal Price { get; init; }
	public IEnumerable<UpdateOrderProductResponse> OrderProduct { get; init; } = [];
}
