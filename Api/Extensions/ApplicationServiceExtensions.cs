using Api.Filters;
using Application.Core;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;

namespace Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options => { options.Filters.Add(new ValidationFilter()); });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = config.GetSection("JwtBearerOptions")["Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = config.GetSection("JwtBearerOptions")["ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = config.GetSection("JwtBearerOptions")["ValidAudience"],
                        ValidateLifetime = true
                    };
                });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"}); });
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCloudinary(new Account(
                config.GetSection("Cloudinary")["CloudName"],
                config.GetSection("Cloudinary")["ApiKey"],
                config.GetSection("Cloudinary")["ApiSecret"]
            ));

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy",
                    policy => { policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin(); });
            });

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddUserServices(config);
            services.AddPassionServices(config);
            services.AddHateServices(config);
            services.AddMatchServices(config);
            services.AddDateServices(config);

            return services;
        }
    }
}