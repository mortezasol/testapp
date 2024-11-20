using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApplication1;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Service registration
    try
    {
        // Add controllers and API documentation
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add custom services
        builder.Services.AddScoped<JwtService>();
        builder.Services.AddDbContext<AppDbContext>(options =>
     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IRepository<TaskCore>, TaskRepository>();
        builder.Services.AddScoped<ITaskService, TaskService>();
        builder.Services.AddScoped<TaskRepository>();

        // Add SignalR for real-time communication
        builder.Services.AddSignalR();

        // Configure JWT authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    RoleClaimType = ClaimTypes.Role 
                };
            });

        // Configure CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
                policy.WithOrigins("http://localhost:3000")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials());
        });

        // Add authorization services
        builder.Services.AddAuthorization();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during service registration: {ex.Message}");
        throw; 
    }

    var app = builder.Build();

    // Application configuration
    try
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enable CORS
        app.UseCors("CorsPolicy");

        // Authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Map controllers and SignalR hubs
        app.MapControllers();
        app.MapHub<TaskHub>("/taskhub");

        // Run the application
        app.Run();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during app configuration: {ex.Message}");
        throw; // Re-throw to ensure visibility of configuration issues
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Critical error: {ex.Message}");
}
