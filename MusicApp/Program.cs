using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MusicApp.Business;
using MusicApp.Business.Services;
using MusicApp.Business.Services.Token;
using MusicApp.Data;
using MusicApp.Data.Repositories;
using MusicApp.Data.Repository; // Adjust namespace if needed
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure the MySQL connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Register controllers
builder.Services.AddControllers();

// Register business services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<ITokenBlacklistService, InMemoryTokenBlacklistService>();


// Register the generic repository. Adjust the namespace if necessary.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Configure JWT authentication.
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };

    // ✅ Add this to check for blacklisted tokens
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var blacklistService = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
            if (blacklistService.IsTokenBlacklisted(token))
            {
                context.Fail("Token is blacklisted.");
            }
            return Task.CompletedTask;
        }
    };
});



builder.Services.AddAuthorization();

// Configure Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS policy.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
