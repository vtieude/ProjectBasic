using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Core;
using Service.Core.API;
using Service.Core.DAL.Models;

namespace Service.Login.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SwaggerConfig = Configuration.GetSection("Swagger").Get<SwaggerConfig>();

            if (SwaggerConfig == null)
            {
                throw new Exception("SwaggerConfig is null");
            }
        }

        public IConfiguration Configuration { get; }
        private SwaggerConfig SwaggerConfig { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // API configuration
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.RegisterSwaggerGen(SwaggerConfig);

            // Routing and error handling           
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Database configuration
            services.AddDbContext<DbContext, FirstContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddUnitOfWork<FirstContext>();

            // Security configuration
            IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
            services.AddAuthentication(appSettingsSection);
            services.ConfigureAuthorization();
            // Custom services
            services.ConfigureServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            // Security
            app.UseCors("DinnerCorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();

            // API documentation
            app.ConfigureSwagger(SwaggerConfig);

            // Route handling                                         
            app.UseMvc();
        }
    }
}
