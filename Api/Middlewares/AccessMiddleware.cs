using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Api.Middlewares
{
    public enum Access {
        Admin,
        Common
    }

    public class RequireAccess : Attribute
    {
        public Access Access { get; set; }

        public RequireAccess(Access access)
        {
            Access = access;
        }
    }
    
    public class AccessMiddleware
    {
        private RequestDelegate _next;

        public AccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IMediator mediator)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<RequireAccess>();

            if (attribute != null)
            {
                var claims = context.User.Claims.ToDictionary(claim => claim.Type);
                
                var userId = claims.GetValueOrDefault("user_id")?.Value;
                var user = await mediator.Send(new ReadOne.Query {Id = userId});
                
                if (user?.AccessLevel != attribute.Access.ToString())
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("");
                    return;
                }
            }

            await _next(context);
        } 
    }
}