using Microsoft.AspNetCore.Hosting;
using Service.Login.API.Area;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace Service.Login.API.Area
{
    #region Using Statements

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Service.Core.DAL.Models;
    using Service.Login.BAL.Model;

    #endregion

    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<IdentityContext>(options =>
                                                           options.UseSqlServer(
                                                               context.Configuration.GetConnectionString("DefaultConnection")));
                services.AddIdentity<AspNetUserInput, IdentityRole>()
                        .AddEntityFrameworkStores<IdentityContext>()
                        .AddDefaultTokenProviders();
            });
        }
    }
}
