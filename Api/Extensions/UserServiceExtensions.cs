using Application.CloudinaryImages;
using Application.Users;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class UserServiceExtensions
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(typeof(Create.Handler));
            services.AddMediatR(typeof(ReadOne.Handler));
            services.AddMediatR(typeof(ReadMany.Handler));
            services.AddMediatR(typeof(Update.Handler));
            services.AddMediatR(typeof(Delete.Handler));
            services.AddMediatR(typeof(Recommend.Handler));
            services.AddMediatR(typeof(AddToUser.Handler));
            return services;
        }
    }
}