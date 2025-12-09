using Microsoft.EntityFrameworkCore;
using MyWebApi.Models;   //  Contains Cloud495Context
// If your Cloud495Context class is actually inside MyWebApi.Data, keep this too:
using MyWebApi.Data;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// Database configuration
// --------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ Register Cloud495Context in DI container
builder.Services.AddDbContext<Cloud495Context>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

// --------------------------------------------------
// CORS configuration
// --------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "https://allcloudsvcs.com",
                "https://brentaneal.com",
                "https://api.brentaneal.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// --------------------------------------------------
// Controllers & Health checks
// --------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

// --------------------------------------------------
// Build the app
// --------------------------------------------------
var app = builder.Build();

// --------------------------------------------------
// Middleware pipeline
// --------------------------------------------------
app.UseCors("AllowFrontend");

app.UseRouting();

// If your API needs HTTPS enforce it, otherwise leave off in ECS Fargate
// app.UseHttpsRedirection();

app.MapControllers();

app.MapHealthChecks("/health");
app.MapHealthChecks("/api/health");

// --------------------------------------------------
// Force Kestrel to bind explicitly on port 80
// --------------------------------------------------
app.Urls.Clear();
app.Urls.Add("http://0.0.0.0:80");

Console.WriteLine(">>> Application bound to http://0.0.0.0:80 <<<");

// --------------------------------------------------
// Run the application
// --------------------------------------------------
app.Run();