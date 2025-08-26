using Common.Authorization;
using Common.Common;
using Common.Exceptions;
using Common.Mapper;
using Common.OpenApi;
using DotNetEnv;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using OnlineStore.API.Contracts.Auth.Examples;
using OnlineStore.Application.Carts;
using OnlineStore.Application.Extensions;
using OnlineStore.Persistance.Extensions;
using Minios;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

Env.Load("./../../.env");

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Host.UseSerilog(
	(context, config) =>
		config.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext());

const bool useSwagger = true;

services
		.AddCommon()
		.AddExceptions()
				.AddAuthorization(configuration)
				.AddMapper();

if (useSwagger)
	services.AddSwagger();
else
	services.AddScalar();

services
		.AddApplication()
		.AddInfrastructure(configuration);

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
services.AddSwaggerExamplesFromAssemblyOf<RegistrationRequestExample>();
services.AddMemoryCache();
services.AddHttpContextAccessor();
services.AddScoped<ICartService, CartService>();

var app = builder.Build();

app.ApplyMigrations();
app.Services.EnsureMinioBucketExistsAsync().GetAwaiter().GetResult();

app.UseExceptionHandler();

if (useSwagger)
{
	app.UseSwagger();
	app.UseSwaggerUI(
		c =>
		{
			c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API v1");
		});
}
else
{
	app.UseScalar();
}

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