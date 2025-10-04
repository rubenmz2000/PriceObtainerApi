using Microsoft.EntityFrameworkCore;
using PriceObtainer.DataAccess.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<PriceObtainerDbContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped
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