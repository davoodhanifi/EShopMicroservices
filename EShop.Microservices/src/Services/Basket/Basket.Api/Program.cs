using Basket.Api.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Services;
using HealthChecks.UI.Client;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddCarter();
var postgresqlConnectionString = builder.Configuration.GetConnectionString("Database")!;
var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

builder.Services.AddMarten(options =>
{
    options.Connection(postgresqlConnectionString);
    options.Schema.For<ShoppingCart>().Identity(x=>x.Username);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    return ConnectionMultiplexer.Connect(redisConnectionString);
});

builder.Services.AddSingleton(sp =>
{
    var connection = sp.GetRequiredService<IConnectionMultiplexer>();
    return connection.GetDatabase();
});

builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddHealthChecks()
    .AddNpgSql(postgresqlConnectionString)
    .AddRedis(redisConnectionString);


var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
        new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

app.Run();
