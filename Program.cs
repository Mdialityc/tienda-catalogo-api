using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Utils.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(x =>
    x.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Auth"));
string key = builder.Configuration["Auth:Key"] ?? throw new InvalidOperationException();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", b =>
    {
        b.AllowAnyOrigin() 
            .AllowAnyHeader() 
            .AllowAnyMethod();
    });
});

builder.Services
    .AddAuthorization()
    .AddFastEndpoints()
    .AddSwaggerGen();

builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "Tienda Catalogo Api";
        s.Version = "v1";
    };
});


var app = builder.Build();

app.UseCors("AllowAllOrigins");

app
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api")
    .UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();