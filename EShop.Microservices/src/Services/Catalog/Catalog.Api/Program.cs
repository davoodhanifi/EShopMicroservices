var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapCarter();
app.Run();
