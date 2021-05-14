using Application.Dates;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class DateServiceExtensions
    {
        public static IServiceCollection AddDateServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(typeof(ReadManyByUser.Handler));
            services.AddMediatR(typeof(ChatWithDate.Handler));
            return services;
        }
    }
}