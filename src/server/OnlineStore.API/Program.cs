using Common.Authorization;
using Common.Common;
using Common.Exceptions;
using Common.Mapper;
using Common.Swagger;
using DotNetEnv;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using OnlineStore.API.Contracts.Examples;
using OnlineStore.Application.Extensions;
using OnlineStore.Persistance.Extensions;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

Env.Load("./../../.env");

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Host.UseSerilog(
    (context, config) =>
        config.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext());

services
        .AddCommon()
        .AddExceptions()
        .AddAuthorization(configuration)
        .AddSwagger()
        .AddMapper();

services
    .AddApplication()
    .AddInfrastructure(configuration);

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
services.AddSwaggerExamplesFromAssemblyOf<RegistrationRequestExample>();

var app = builder.Build();

app.ApplyMigrations();

app.UseExceptionHandler();

app.UseSwagger();

app.UseSwaggerUI(
    c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API v1");
    });

app.UseCookiePolicy(
    new CookiePolicyOptions
    {
        MinimumSameSitePolicy = SameSiteMode.None,
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always
    });

app.UseHttpsRedirection();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.All
    });
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();