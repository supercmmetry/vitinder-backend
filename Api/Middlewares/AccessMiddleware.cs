using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.Core;
using Application.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;

namespace Api.Middlewares
{
    public enum Access
    {
        Admin = 10,
        Common = 1,
        Anonymous = 0
    }

    public class RequireAccessAttribute : Attribute
    {
        public Access Access { get; set; }

        public RequireAccessAttribute(Access access)
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
            var attribute = endpoint?.Metadata.GetMetadata<RequireAccessAttribute>();

            if (attribute != null)
            {
                var claims = context.User.Claims.ToDictionary(claim => claim.Type);

                var userId = claims.GetValueOrDefault("user_id")?.Value;
                
                var user = attribute.Access > Access.Anonymous
                    ? await mediator.Send(new ReadOne.Query {Id = userId})
                    : null;

                if (user?.AccessLevel < (int) attribute.Access)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
                    {
                        Metadata = ErrorMetadata.InsufficientAuthorization,
                        Message = "The given operation requires " + attribute.Access
                    }));
                    return;
                }

                context.Items["UserId"] = userId;
                context.Items["User"] = user;
            }

            await _next(context);
        }
    }
}