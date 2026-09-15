using EngagementApi.Application;
using EngagementApi.Application.Common.Interfaces;
using EngagementApi.Infrastructure;
using EngagementApi.Infrastructure.DiExtensions;
using EngagementApi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicyName = "FrontendPolicy";

builder.Services.AddControllers();

builder.Services.AddSwaggerServicesExtensions();

builder.Services.AddApplication();

var uploadsRootPath = Path.Combine(
    builder.Environment.WebRootPath ?? builder.Environment.ContentRootPath,
    "uploads");

builder.Services.AddInfrastructure(
    builder.Configuration,
    uploadsRootPath);

var jwtSection = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSection["Key"]
    ?? throw new InvalidOperationException(
        "Jwt:Key is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],

        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)),

        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        var origins =
            builder.Configuration
                .GetSection("Cors:AllowedOrigins")
                .Get<string[]>()
            ?? new[] { "http://localhost:4200" };

        policy
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    var passwordHasher = scope.ServiceProvider
        .GetRequiredService<IPasswordHasherService>();

    await DbSeeder.SeedAsync(
        db,
        passwordHasher,
        app.Configuration);
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Swagger
app.AddSwaggerApplicationExtensions();

app.UseCors(CorsPolicyName);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();