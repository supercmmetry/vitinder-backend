using Application.Matches;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class MatchServiceExtensions
    {
        public static IServiceCollection AddMatchServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddMediatR(typeof(Create.Handler));
            services.AddMediatR(typeof(ReadOne.Handler));
            services.AddMediatR(typeof(ReadMany.Handler));
            services.AddMediatR(typeof(Update.Handler));
            services.AddMediatR(typeof(Delete.Handler));
            services.AddMediatR(typeof(ReadManyByOther.Handler));
            return services;
        }
    }
}