using Assignment_DotNet6.Data;
using Assignment_DotNet6.Entities;
using Assignment_DotNet6.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Assignment_DotNet6.Core
{
    public static class ServiceExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connection = configuration.GetConnectionString("connectionString") ?? throw new ArgumentNullException("Missing connectionString.");

            services.AddDbContext<DataContext>(options =>
            {
                options.UseMySql(connection,ServerVersion.AutoDetect(connection));
            });

            #region Auhentication
            services.AddIdentity<Doctor, IdentityRole>()
               .AddEntityFrameworkStores<DataContext>()
               .AddDefaultTokenProviders();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(3);

            })
            .AddJwtBearer(options =>
               {
                   options.SaveToken = true;
                   options.RequireHttpsMetadata = false;
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       //ValidAudience = JWT_Audiance,
                       //ValidIssuer = JWT_Issuer,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x323423@$%@pyw349as@")),
                   };
               });

            services.AddTransient<DBService>();

            services.AddCors(options =>
            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        //.WithOrigins("http://10.10.20.35:3000")
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                });
            });

            #endregion

            return services;
        }
    }
}
