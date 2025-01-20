using Ordering.Api.Extensions;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices()
    .AddApiServices();

var app = builder.Build();

app.UseApiServices();

app.MapGet("/", () => "Hello World!");

app.Run();
