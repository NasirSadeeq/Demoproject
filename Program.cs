using Demoproject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContaxt>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddScoped<IadminRepository, AdminRepository>();
builder.Services.AddScoped<IuserRepository,UserRepositry>();
builder.Services.AddScoped<IactivityRepository, ActivityRepository>();
//builder.Services.AddSingleton(typeof(IwjtTokenManager),typeof(JwtTokenManager) ) ();

builder.Services.AddAuthentication(authenoption =>
{
    authenoption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authenoption.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwtoptions =>
    {

        var Key = builder.Configuration.GetValue<string>("JwtConfig:Key");
        var KeyBytes = Encoding.ASCII.GetBytes(Key);
        jwtoptions.SaveToken = true;
        jwtoptions.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(KeyBytes),
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew=TimeSpan.Zero
        };
    });
builder.Services.AddScoped(typeof(IwjtTokenManager), typeof(JwtTokenManager));
//builder.Services.AddScoped(typeof(IadminTokenManager), typeof(AdminTokenManager));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
