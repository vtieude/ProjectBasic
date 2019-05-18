using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Login.API
{
    public static class ServiceAuthorization
    {
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            // Authorization claim policies
            services.AddAuthorization(options =>
            {
                //Users
               
                //Roles
                
            });
        }
    }
}
