using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Context;
using Domain.User;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.IdentityConfigs
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services , IConfiguration configuration)
        {
            string connection = configuration["ConnectionString:SqlDbConnection"];
            services.AddDbContext<IdentityDatabaseContext>(option => option.UseSqlServer(connection));
            
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDatabaseContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<CustomeIdentityError>();


            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            });
            return services;
        }
    }
}
