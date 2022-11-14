using Financity.Application;
using Financity.Application.Abstractions.Data;
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


JwtConfiguration.Register(builder.Services, builder.Configuration);

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
               var jwtConfiguration = JwtConfiguration.GetInstance();

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidAlgorithms = new[] { jwtConfiguration.Algorithm },
                   ValidateIssuer = jwtConfiguration.ValidateIssuer,
                   ValidateAudience = jwtConfiguration.ValidateAudience,
                   ValidateLifetime = jwtConfiguration.ValidateLifetime,
                   ValidateIssuerSigningKey = jwtConfiguration.ValidateIssuerSigningKey,
                   ValidIssuer = jwtConfiguration.ValidIssuer,
                   ValidAudience = jwtConfiguration.ValidAudience,
                   IssuerSigningKey = jwtConfiguration.IssuerSigningKey
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

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.MapControllers().RequireAuthorization("Api");
app.MapHealthChecks("/api/healthcheck");

app.Run();