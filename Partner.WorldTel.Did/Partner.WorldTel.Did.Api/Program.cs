using Microsoft.EntityFrameworkCore;
using Partner.WorldTel.Did.Api.Data;
using Partner.WorldTel.Did.Api.Interface;
using Partner.WorldTel.Did.Api.Service;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services
    .AddDbContext<AppDbContext>(options =>
    options
    .UseSqlite(builder
    .Configuration
    .GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IDidGeneratorService, DidGeneratorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
