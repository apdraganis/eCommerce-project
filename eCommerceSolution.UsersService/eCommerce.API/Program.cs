using eCommerce.Infrastructure;
using eCommerce.Core;
using eCommerce.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure services
builder.Services.AddInfrastructure();
builder.Services.AddCore();

builder.Services.AddControllers();

// Build the application
var app = builder.Build();

// Routing
app.UseRouting();

// Middleware
app.UseExceptionHandllingMiddleware();

// Auth
app.UseAuthentication(); 
app.UseAuthorization();

// Controller routes
app.MapControllers();

app.Run();
