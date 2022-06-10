using SecureMessengerBohdan.Extensions;
using SecureMessengerBohdan.Filters;
using SecureMessengerBohdan.Identity.Extensions;
using SecureMessengerBohdan.Identity.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.SetSettings();
builder.SetMongoIdentitySettings();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<WrappedActionFilter>();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await SeedDb.Invoke(scope.ServiceProvider);
}

app.UseCors(
    cfg => cfg.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
