using System.Security.Claims;
using System.Text;
using Application.Services;
using Application.Validator.Motorcycle;
using Core.Entities;
using Core.Interfaces.Email;
using Core.Interfaces.RepositoriesInterfaces;
using Core.Interfaces.Security;
using Core.Security;
using Infrastructure.Data.Repositories;
using Infrastructure.Email;
using Infrastructure.Middleware;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using Application.Validator.User;
using Core.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.ConfigureValidators();
builder.ConfigureAuthentication();
builder.ConfigureRepositories();
builder.ConfigureOptions();
builder.ConfigureApplicationServices();

var app = builder.Build();

// Configurar pipeline de solicitudes HTTP
app.UseHttpsRedirection();
app.MapOpenApi();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.MapControllers();

app.Run();

// Métodos de extensión para organizar la configuración
internal static class ServiceCollectionExtensions
{
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                ClockSkew = TimeSpan.Zero,
                
                RoleClaimType = ClaimTypes.Role
            };
        });
        
        builder.Services.AddAuthorization();

    }

    public static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserRepository>(provider =>
            new UserRepository(builder.Configuration.GetConnectionString("Postgres")!));
        
        builder.Services.AddScoped<IRefreshTokenRepository>(provider =>
            new RefreshTokenRepository(builder.Configuration.GetConnectionString("Postgres")!));
        
        builder.Services.AddScoped<IVerificationCodesRepository>(provider =>
            new VerificationCodesRepository(builder.Configuration.GetConnectionString("Postgres")!));
        
        builder.Services.AddScoped<IMotorcycleRepository>(provider =>
            new MotorcycleRepository(builder.Configuration.GetConnectionString("Postgres")!));
    }

    public static void ConfigureOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
    }
    
    public static void ConfigureValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ResetPasswordValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<VerifyUserValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ChangeStatusValidator>();
        
        builder.Services.AddValidatorsFromAssemblyContaining<CreateMotorcycleValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateMotorcycleValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<FilterMotorcycleValidator>();
    }

    public static void ConfigureApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IJWTService, JWTService>();
        builder.Services.AddScoped<JWTService>();  
        builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IMotorcycleService, MotorcycleService>();
        
        builder.Services.AddScoped<IEmailService, EmailService>();
    }
}