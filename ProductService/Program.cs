using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductService.Consumers;
using ProductService.Data;
using ProductService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MassTransit
builder.Services.AddMassTransit(x =>
{
	x.AddConsumer<OrderCreatedConsumer>();
	x.AddConsumer<OrderDeletedConsumer>();

	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
		{
			h.Username(builder.Configuration["RabbitMq:Username"]);
			h.Password(builder.Configuration["RabbitMq:Password"]);
		});

		cfg.ReceiveEndpoint("order-created-event", e =>
			e.ConfigureConsumer<OrderCreatedConsumer>(context));

		cfg.ReceiveEndpoint("order-deleted-event", e =>
			e.ConfigureConsumer<OrderDeletedConsumer>(context));
	});
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();
