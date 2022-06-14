using Microsoft.AspNetCore.Http.Connections;
using SecureMessengerBohdan.Extensions;
using SecureMessengerBohdan.Filters;
using SecureMessengerBohdan.Hubs;
using SecureMessengerBohdan.Identity.Extensions;
using SecureMessengerBohdan.Identity.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.SetSettings();
builder.SetMongoIdentitySettings();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<WrappedActionFilter>();
}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = ValidationExceptionResponseHelper.Factory;
});

builder.Services.AddSignalR();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await SeedDb.Invoke(scope.ServiceProvider);
}

app.UseCors(cfg =>
{
    cfg.AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200")
    .AllowCredentials();
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub", options =>
    {
        options.Transports =
                HttpTransportType.WebSockets |
                HttpTransportType.LongPolling;
    });
});

app.Run();
