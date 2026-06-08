using System.Text;
using Azure.Messaging.ServiceBus;
using JWTAuthentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

// 1. Initialize the single, modern builder instance
var builder = FunctionsApplication.CreateBuilder(args);

// NOTE: ConfigureFunctionsWebApplication is an extension that targets Microsoft.Extensions.Hosting.IHostBuilder.
// FunctionsApplicationBuilder implements IHostApplicationBuilder / IFunctionsWorkerApplicationBuilder and
// therefore does not have that extension available. If your intent is to configure the Functions worker
// middleware pipeline, use the IFunctionsWorkerApplicationBuilder APIs (or obtain an IHostBuilder).
//
// If you want to configure worker middleware here, provide the pipeline via builder.Use(...) or
// adjust to an IHostBuilder-based setup. Example placeholder below shows where to add worker middleware.

// Example: configure functions worker middleware if available on this builder implementation:
// builder.Use(next => context =>
// {
//     // middleware logic here
//     return next(context);
// });

// 3. Register your shared ecosystem services
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// 4. Inject your JWT Bearer token authentication

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//JwtExtensions.AddJwtAuthentication(builder.Services, builder.Configuration);


//var jwt = builder.Configuration.GetSection("JWT");

//    var Secret = jwt.GetSection("Secret").Value;
//    var Issuer = jwt.GetSection("Issuer").Value;
//    var Audience = jwt.GetSection("Audience").Value;

//    builder.Services.AddAuthentication(options =>
//    {
//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//    })

//    .AddJwtBearer("Bearer", options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = Issuer,
//            ValidateAudience = true,
//            ValidAudience = Audience,
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
//            ValidateLifetime = true,   // ensures expired tokens are rejected
//            ClockSkew = TimeSpan.Zero  // no extra tolerance for expiration
//        };
//    });






// 5. Build and execute the unified application host exactly ONCE
var host = builder.Build();
host.Run();