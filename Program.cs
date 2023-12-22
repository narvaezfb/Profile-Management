using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Profile_Management.Data;
using Profile_Management.Interfaces;
using Profile_Management.Middleware;
using Profile_Management.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.

// Configure DbContext 
builder.Services.AddDbContext<ProfileManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Add JWT Bearer authentication information for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Include the JWT token in the Swagger UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Register TokenValidationServices
builder.Services.AddHttpClient<ITokenValidationService, TokenValidationService>();

var app = builder.Build();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/User/Account"), appBuilder =>
{
    appBuilder.UseMiddleware<TokenValidationMiddleware>();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

