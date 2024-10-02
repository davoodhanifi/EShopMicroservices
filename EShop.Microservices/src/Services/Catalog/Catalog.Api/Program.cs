var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapCarter();
app.Run();
