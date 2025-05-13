using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly ProductDbContext _context;

		public ProductsController(ProductDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll() => Ok(await _context.Products.ToListAsync());

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var product = await _context.Products.FindAsync(id);
			return product == null ? NotFound() : Ok(product);
		}

		[HttpPost]
		public async Task<IActionResult> Create(ProductCreateDto productdto)
		{
			Product product = new Product();
			product.Id = Guid.NewGuid();
			product.Name = productdto.Name;
			product.Quantity = productdto.Quantity;
			product.Price = productdto.Price;

			_context.Products.Add(product);
			await _context.SaveChangesAsync();
		    return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
			// return Ok(product);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(Guid id, ProductUpdateDto updated)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();

			product.Name = updated.Name;
			product.Quantity = updated.Quantity.Value;
			product.Price = updated.Price.Value;

			await _context.SaveChangesAsync();
			return NoContent();
			// return Ok(product);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return NoContent();
		}

		// Reducing quantity on order creation
/*		[HttpPut("reduce-quantity/{id}")]
		public async Task<IActionResult> ReduceQuantity(Guid id, [FromQuery] int quantity)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null || product.Quantity < quantity)
				return BadRequest("Invalid product or insufficient quantity");

			product.Quantity -= quantity;
			await _context.SaveChangesAsync();
			return Ok();
		}*/

		// Adjusting quantity on update
		[HttpPut("adjust-quantity/{id}")]
		public async Task<IActionResult> AdjustQuantity(Guid id, [FromQuery] int difference)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();

			if (product.Quantity - difference < 0)
				return BadRequest("Resulting quantity cannot be negative");

			product.Quantity -= difference;
			await _context.SaveChangesAsync();
			return Ok();
		}

		// Restoring quantity on deletion
		[HttpPut("restore-quantity/{id}")]
		public async Task<IActionResult> RestoreQuantity(Guid id, [FromQuery] int quantity)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();

			product.Quantity += quantity;
			await _context.SaveChangesAsync();
			return Ok();
		}

	}
}
