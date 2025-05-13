namespace OrderService.Models
{
	public class OrderRequestDto
	{
		public Guid ProductId { get; set; }
		public int Quantity { get; set; }

	}
}
