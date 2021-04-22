using Application.Hates;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class HateServiceExtensions
    {
        public static IServiceCollection AddHateServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(typeof(Create.Handler));
            services.AddMediatR(typeof(ReadOne.Handler));
            services.AddMediatR(typeof(ReadMany.Handler));
            services.AddMediatR(typeof(Update.Handler));
            services.AddMediatR(typeof(Delete.Handler));
            services.AddMediatR(typeof(SetInUser.Handler));
            return services;
        }
    }
}