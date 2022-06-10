using SecureMessengerBohdan.Settings;

namespace SecureMessengerBohdan.Extensions
{
    public static class WebApplicationBuilderSetSettingsExtensions
    {
        public static WebApplicationBuilder SetSettings(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<MongoDbConfig>(conf =>
            builder.Configuration.GetSection(nameof(MongoDbConfig))
            .Bind(conf));

            return builder;
        }
    }
}
