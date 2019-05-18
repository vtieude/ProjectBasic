using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Service.Core.API
{
    public class SwaggerConfig
    {
        public string ContactEmail { get; set; } = "wilson@gmail.com";
        public string ContactName { get; set; } = "Dinner Development";
        public string ContactUrl { get; set; } = "";

        public string Description { get; set; } = "";

        // Settings
        public bool IsEnabled { get; set; } = true;
        public string LicenseName { get; set; } = "";
        public string LicenseUrl { get; set; } = "";
        public string TermsOfService { get; set; } = "None";
        public string Title { get; set; } = "Service API";
        public bool UiIsEnabled { get; set; } = true;
        public string UiRoutePrefix { get; set; } = "swagger";
        public string UiTitle { get; set; } = "Swagger UI";

        // Information
        public string Version { get; set; } = "v1";
    }
    public static class SwaggerConfigExtensions
    {
        public static void ConfigureSwagger(this IApplicationBuilder app, SwaggerConfig swaggerConfig)
        {
            if (!swaggerConfig.IsEnabled)
            {
                return;
            }
            app.UseSwagger(c =>
            {
                c.RouteTemplate = swaggerConfig.UiRoutePrefix + "/{documentName}/swagger.json";
            });
            if (swaggerConfig.UiIsEnabled)
            {
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint($"/{swaggerConfig.UiRoutePrefix}/{swaggerConfig.Version}/swagger.json", swaggerConfig.UiTitle);
                    options.RoutePrefix = swaggerConfig.UiRoutePrefix;
                    options.DisplayOperationId();
                });
            }
        }
        public static void RegisterSwaggerGen(this IServiceCollection services, SwaggerConfig swaggerConfig)
        {
            if (swaggerConfig.IsEnabled)
            {
                services.AddSwaggerGen(options =>
                {
                    options.MapType<FileContentResult>(() => new Schema
                    {
                        Type = "file"
                    });

                    options.AddSecurityDefinition("oauth2", new ApiKeyScheme
                    {
                        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                        In = "header",
                        Name = "Authorization",
                        Type = "apiKey"
                    });

                    options.OperationFilter<SecurityRequirementsOperationFilter>();

                    options.SwaggerDoc(swaggerConfig.Version, new Info
                    {
                        Title = swaggerConfig.Title,
                        Version = swaggerConfig.Version,
                        Description = swaggerConfig.Description,
                        TermsOfService = swaggerConfig.TermsOfService,
                        Contact = new Contact
                        {
                            Name = swaggerConfig.ContactName,
                            Email = swaggerConfig.ContactEmail,
                            Url = swaggerConfig.ContactUrl
                        },
                        License = new License
                        {
                            Name = swaggerConfig.LicenseName,
                            Url = swaggerConfig.LicenseUrl
                        }
                    });

                    var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);


                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                });
            }
        }
    }

}

