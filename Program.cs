using Microsoft.EntityFrameworkCore;
using QS.Data;
using QS.Models;
using QS.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize object references correctly to handle circular references if needed
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// Register custom services
builder.Services.AddScoped<ReponseValidator>();

// Configure CORS (important to configure before UseAuthorization)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure the DbContext to use SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services for the application
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IMedicalServiceService, MedicalServiceService>();

// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations and seed initial data (only once at startup)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Apply any pending migrations
        SeedData.Initialize(context); // Seed initial data
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the DB.");
    }
}

// Configure the HTTP request pipeline for Swagger and Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger UI in development
    app.UseSwaggerUI();
}

// 🚨 Important: CORS middleware must come before UseAuthorization to allow cross-origin requests
app.UseCors("AllowAll");

// HTTPS redirection and authorization
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers to handle incoming requests
app.MapControllers();

// Start the application
app.Run();
