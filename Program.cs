using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Mock_AWS_API.Configuration;
using Mock_AWS_API.Interface;
using Mock_AWS_API.Repos;
using Mock_AWS_API.Services;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data.Entity;
using Mock_AWSRDS_API.Helper;
using Mock_AWSRDS_API.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Add default AWS options
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// External Services
builder.Services.AddAWSService<IAmazonS3>();

// Services
builder.Services.AddScoped<IMockService, MockService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();

builder.Services.AddDbContext<MyDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your_issuer",
            ValidAudience = "your_audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
        };
    });
var app = builder.Build();

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
