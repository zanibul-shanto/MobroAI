using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobroLens;
using MobroLens.Endpoints;
using MobroLens.Services;
using Resend;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
var testDbName = "TestDb_" + Guid.NewGuid();
var provider = builder.Configuration["DatabaseProvider"] ?? "SqlServer";
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsEnvironment("Testing"))
        options.UseInMemoryDatabase(testDbName);
    else if (provider == "PostgreSQL")
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    else
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<IOnnxInferenceService, OnnxInferenceService>();
builder.Services.AddSingleton<IResend>(_ =>
    ResendClient.Create(builder.Configuration["Resend:ApiKey"] ?? ""));
builder.Services.AddScoped<IEmailService, EmailService>();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Auth
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// APIs
app.MapTodoEndpoints();
app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapChildEndpoints();
app.MapLocationLogEndpoints();
app.MapMeaslesScanEndpoints();


//Start the project
app.Run();

public partial class Program { }