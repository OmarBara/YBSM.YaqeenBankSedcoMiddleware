using System;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Infrastructure.Services.Helper;
using Infrastructure.Services.Helper.passwordHasher;
using Infrastructure.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public static class ServiceContainer
    {
        public static void AddServicesRegistration(this IServiceCollection services, IConfiguration configuration)
        {
           /* services.AddTransient<CategoryServices>();
            services.AddTransient<StoreServices>();
            services.AddTransient<MerchantServices>();
            services.AddTransient<ActivityServices>();
            services.AddTransient<SharedServices>();*/
            services.AddTransient<UserServices>();

            services.AddTransient<CacheProvider>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();


            var emailConfig = configuration
                .GetSection("OtpOptions")
                .Get<OtpOptions>();
            services.AddSingleton(emailConfig);

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                context.Response.Headers.Add("Token-Expired", "true");
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}