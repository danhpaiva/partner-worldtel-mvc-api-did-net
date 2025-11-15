using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Partner.WorldTel.Did.Api.Data;
using Partner.WorldTel.Did.Api.Interface;
using Partner.WorldTel.Did.Api.Service;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.OpenApi.Models; // <- ESSENCIAL

var builder = WebApplication.CreateBuilder(args);

// === DB ===
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AppDbContext")!));

// === CONTROLLERS ===
builder.Services.AddControllers();

// === SERVIÇOS ===
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDidGeneratorService, DidGeneratorService>();

// === JWT ===
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("Jwt:SecretKey não configurado.");

if (secretKey.Length < 16)
    throw new InvalidOperationException("SecretKey deve ter pelo menos 16 caracteres.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// === SWAGGER + SCALAR (SWASHBUCKLE) ===
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorldTel DID API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira apenas o token JWT (sem 'Bearer')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// === PIPELINE ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorldTel DID API v1");
        c.RoutePrefix = "swagger";
    });

    app.MapScalarApiReference(
        options =>
        {
            options.Title = "WorldTel DID API - Scalar";
            options.OpenApiRoutePattern = "/swagger/v1/swagger.json"; // ESSA É A CHAVE!);
        });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();