using SecureMessengerBohdan.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.SetSettings();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
