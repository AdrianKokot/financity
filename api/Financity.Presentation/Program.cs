using System.Text;
using Financity.Application;
using Financity.Domain.Entities;
using Financity.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration)
       .AddApplication(builder.Configuration)
       .AddRouting(options =>
       {
           options.LowercaseUrls = true;
           options.LowercaseQueryStrings = true;
       })
       .AddAuthentication(options =>
       {
           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
       })
       .AddJwtBearer(
           options =>
           {
               var key = new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(
                       builder.Configuration["JwtSettings:TokenValidationParameters:IssuerSigningKey"]));

               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = false,
                   ValidateIssuerSigningKey = false,
                   ValidIssuer = builder.Configuration["JwtSettings:TokenValidationParameters:ValidIssuer"],
                   ValidAudience = builder.Configuration["JwtSettings:TokenValidationParameters:ValidAudience"],
                   IssuerSigningKey = key
               };

           });
builder.Services.AddAuthorization();

builder.Services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Financity",
        Version = "v1"
    });
});

builder.Services.AddHealthChecks();

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

app.UseDefaultFiles();

app.UseSerilogRequestLogging();

app.UseStaticFiles();



app.MapControllers();
app.MapHealthChecks("/api/healthcheck");

app.Run();