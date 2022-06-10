using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Identity.Helpers;
using SecureMessengerBohdan.Identity.Models;
using SecureMessengerBohdan.Identity.Settings;
using System.Text;

namespace SecureMessengerBohdan.Identity.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder SetMongoIdentitySettings(this WebApplicationBuilder builder)
        {

            var settings = builder.Configuration
                .GetSection(nameof(MongoDbConfig))
                .Get<MongoDbConfig>();
            var jwtSettings = builder.Configuration
                .GetSection(nameof(JwtConfig))
                .Get<JwtConfig>();

            builder.Services.AddScoped<TokenWriter>();
            builder.Services
                .AddIdentity<ApplicationUser, ApplicationRole>(conf =>
                {
                    conf.Password.RequireNonAlphanumeric = false;
                    conf.Password.RequireUppercase = false;
                    conf.Password.RequireDigit = false;
                    conf.Password.RequireLowercase = false;
                    conf.SignIn.RequireConfirmedAccount = false;
                })
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(settings.ConnectionString, 
                settings.Name);
            
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                });
            builder.Services.AddAuthorization();
            return builder;
        }
    }
}
