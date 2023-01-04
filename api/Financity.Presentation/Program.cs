using Financity.Application;
using Financity.Application.Abstractions.Data;
using Financity.Application.Common.Configuration;
using Financity.Infrastructure;
using Financity.Persistence.Seed;
using Financity.Presentation.Auth;
using Financity.Presentation.Auth.Configuration;
using Financity.Presentation.Middleware;
using Financity.Presentation.QueryParams;
using Financity.Presentation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

// Configurations
builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(JwtConfiguration.ConfigurationKey));
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection(EmailConfiguration.ConfigurationKey));

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
       .AddCors(options =>
       {
           options.AddDefaultPolicy(c =>
           {
               c.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://financity.fly.dev")
                .AllowAnyMethod().WithHeaders(HeaderNames.ContentType);
           });
       });

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddAuthentication(options =>
       {
           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
       })
       .AddJwtBearer();

builder.Services.ConfigureOptions<ConfigureJwtBearerOptions>();
builder.Services.ConfigureOptions<ConfigureDataProtectionTokenProviderOptions>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthPolicy.Api, policy => { policy.RequireAuthenticatedUser(); });
});

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
    options.ModelBinderProviders.Insert(0, new QuerySpecificationBinderProvider());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<QuerySpecificationFilter>();

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

builder.Services.AddSpaStaticFiles(opt => { opt.RootPath = "wwwroot"; });

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

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers().RequireAuthorization(AuthPolicy.Api);
app.MapHealthChecks("/api/healthcheck");

app.UseDefaultFiles();
app.UseSpaStaticFiles();

app.UseSpa(config => { });

using (var scope = app.Services.CreateScope())
{
    await DataSeeder.RequestExternalApiForCurrencies(scope.ServiceProvider.GetRequiredService<IExchangeRateService>(),
        scope.ServiceProvider.GetRequiredService<IApplicationDbContext>());
}

app.Run();