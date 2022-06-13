using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Identity.Settings;
using System.Reflection;
using MediatR;
using SecureMessengerBohdan.Application.Services;

namespace SecureMessengerBohdan.Extensions
{
    public static class WebApplicationBuilderSetSettingsExtensions
    {
        public static WebApplicationBuilder SetSettings(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<MongoDbConfig>(
                conf => 
                builder.Configuration.GetSection(nameof(MongoDbConfig))
                .Bind(conf));

            builder.Services.Configure<JwtConfig>(
                conf =>
                builder.Configuration.GetSection(nameof(JwtConfig))
                .Bind(conf));
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<CurrentUserService>();
            builder.Services.AddScoped<ApplicationDbContext>();

            return builder;
        }
    }
}
