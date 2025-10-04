using Microsoft.EntityFrameworkCore;
using PriceObtainer.Api.Workers;
using PriceObtainer.DataAccess.Context;
using PriceObtainer.DataAccess.Repositories;
using PriceObtainer.Library.Abstractions;
using PriceObtainer.Library.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<PriceObtainerDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("binance", c =>
{
    c.BaseAddress = new Uri("https://api.binance.com");
    c.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHostedService<PriceIngestionWorker>();
builder.Services.AddScoped<PriceService>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PriceObtainerDbContext>();
    db.Database.Migrate();
}

app.MapControllers();

app.UseHttpsRedirection();

app.Run();