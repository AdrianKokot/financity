using Financity.Application;
using Financity.Application.Abstractions.Configuration;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Exceptions;
using Financity.Infrastructure;
using Financity.Presentation.Auth;
using Financity.Presentation.Configuration;
using Financity.Presentation.Middleware;
using Financity.Presentation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

// Configurations
var emailConfig = builder.Configuration.GetSection(EmailConfiguration.ConfigurationKey).Get<EmailConfiguration>()
                  ?? throw new MissingConfigurationException(EmailConfiguration.ConfigurationKey);
builder.Services.AddSingleton<IEmailConfiguration>(emailConfig);

var jwtConfig = builder.Configuration.GetSection(JwtConfiguration.ConfigurationKey)
                       .Get<JwtConfiguration>()
                ?? throw new MissingConfigurationException(JwtConfiguration.ConfigurationKey);
builder.Services.AddSingleton<IJwtConfiguration>(jwtConfig);

// Add services to the container.
builder.Services
       .AddTransient<ExceptionHandlingMiddleware>()
       .AddApplication(builder.Configuration)
       .AddInfrastructure(builder.Configuration)
       .AddRouting(options =>
       {
           options.LowercaseUrls = true;
           options.LowercaseQueryStrings = true;
       })
       .AddCors(options => { options.AddDefaultPolicy(c => { c.AllowAnyOrigin(); }); });

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services
       .AddAuthentication(options =>
       {
           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
       })
       .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
           options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidAlgorithms = new[] { jwtConfig.Algorithm },
                   ValidateIssuer = jwtConfig.ValidateIssuer,
                   ValidateAudience = jwtConfig.ValidateAudience,
                   ValidateLifetime = jwtConfig.ValidateLifetime,
                   ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                   ValidIssuer = jwtConfig.ValidIssuer,
                   ValidAudience = jwtConfig.ValidAudience,
                   IssuerSigningKey = jwtConfig.IssuerSigningKey
               };
           })
       .AddAuthConfiguration();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Api", policy => { policy.RequireAuthenticatedUser(); });
});

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

    var securityScheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
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

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers().RequireAuthorization("Api");
app.MapHealthChecks("/api/healthcheck");

app.Run();