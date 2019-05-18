using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.Core.DAL;
using Service.Core.DAL.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public static class CoreConfiguration
    {
        public static IServiceCollection AddLmsAuthentication(this IServiceCollection services, IConfigurationSection configuration)
        {
            // Allow cross site communication
            services.AddCors(o => o.AddPolicy("LMSCorsPolicy",
                                              builder =>
                                                  builder
                                                      .SetIsOriginAllowed(host => true)
                                                      .AllowAnyMethod()
                                                      .AllowAnyHeader()
                                                      .AllowCredentials()));

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            // Strongly typed settings object
            services.Configure<TokenSettings>(configuration);
            TokenSettings tokenSettings = configuration.Get<TokenSettings>();

            // JWT authentication         
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(cfg =>
                    {
                        cfg.RequireHttpsMetadata = true;
                        cfg.SaveToken = true;

                        cfg.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = tokenSettings.Issuer,
                            ValidateAudience = false,
                            ValidAudience = tokenSettings.Audience,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Secret))
                        };
                    });

            return services;
        }

        public static IServiceCollection AddUnitOfWork<T>(this IServiceCollection services) where T : DbContext
        {
            services.AddScoped(typeof(IUserContext), typeof(HttpUserContext));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient(typeof(IUnitOfWorkFactory), serviceProvider =>
                                      new UnitOfWorkFactory<T>(serviceProvider.GetService<T>, serviceProvider.GetService<IUserContext>()));

            return services;
        }
    }
}
