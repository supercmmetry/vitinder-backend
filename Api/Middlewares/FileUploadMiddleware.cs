using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
    public class ValidateFileAttribute : Attribute
    {
        public readonly string FileKey;

        public readonly ICollection<string> AllowedExtensions;

        public readonly int MaxSize;

        public ValidateFileAttribute(string fileKey, int maxSize, params string[] allowedExtensions)
        {
            FileKey = fileKey;
            AllowedExtensions = allowedExtensions;
            MaxSize = maxSize;
        }
    }

    public class ValidateFileMiddleware
    {
        private RequestDelegate _next;

        public ValidateFileMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IMediator mediator)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<ValidateFileAttribute>();

            if (attribute != null)
            {
                var file = context.Request.Form.Files.GetFile(attribute.FileKey);

                if (file == null)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
                    {
                        Metadata = ErrorMetadata.MissingResource,
                        Message = "Failed to resolve ProfilePicture from request."
                    }));
                    return;
                }
                
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (attribute.AllowedExtensions != null && !attribute.AllowedExtensions.Contains(fileExt))
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
                    {
                        Metadata = ErrorMetadata.InvalidResource,
                        Message = "Invalid file extension: " + fileExt
                    }));
                    return;
                }

                if (file.Length > attribute.MaxSize)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponse
                    {
                        Metadata = ErrorMetadata.InvalidResource,
                        Message = "File size should not exceed " + attribute.MaxSize + " bytes"
                    }));
                    return;
                }

                if (context.Items["ValidatedFiles"] == null)
                {
                    context.Items["ValidatedFiles"] = new FormFileCollection();
                }

                var validatedFiles = context.Items["ValidatedFiles"] as FormFileCollection;
                validatedFiles?.Add(file);
            }

            await _next(context);
        }
    }
}