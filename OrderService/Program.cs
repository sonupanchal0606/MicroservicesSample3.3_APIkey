using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrderDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// 1st way ----> using full URL
// builder.Services.AddHttpClient();

// 2nd way ---> using Iconfiguration
builder.Services.AddHttpClient();

// 3rd way ----> creating base url here
/*builder.Services.AddHttpClient("ProductService", client =>
{
	client.BaseAddress = new Uri("https://localhost:5001/"); // product service base address
});*/

// OR

// to use product uri from appsettings.json
/*builder.Services.AddHttpClient("ProductService", client =>
{
	client.BaseAddress = new Uri(builder.Configuration["Services:Product"]);
});*/

builder.Services.AddMassTransit(x =>
{
	x.UsingRabbitMq((context, cfg) =>
	{
		cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
		{
			h.Username(builder.Configuration["RabbitMq:Username"]);
			h.Password(builder.Configuration["RabbitMq:Password"]);
		});
	});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
