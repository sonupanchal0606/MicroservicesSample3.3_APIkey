using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using Shared.Messages;

namespace ProductService.Consumers
{
	public class OrderDeletedConsumer : IConsumer<OrderDeleted>
	{
		private readonly ProductDbContext _context;

		public OrderDeletedConsumer(ProductDbContext context)
		{
			_context = context;
		}

		public async Task Consume(ConsumeContext<OrderDeleted> context)
		{
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == context.Message.ProductId);
			if (product != null)
			{
				product.Quantity += context.Message.Quantity;
				await _context.SaveChangesAsync();
			}
		}
	}
}

