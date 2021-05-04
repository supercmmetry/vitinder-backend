using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Application.Core
{
    public enum ErrorMetadata
    {
        MissingResource,
        InvalidResource,
        InsufficientAuthorization,
        ResourceAlreadyExists,
    }
    
    public class ErrorResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorMetadata Metadata;
        
        public string Message;

        public ErrorResponse()
        {
        }

        public ErrorResponse(ErrorMetadata metadata, string message)
        {
            Metadata = metadata;
            Message = message;
        }
    }

    public class DetailedException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ErrorResponse ErrorResponse { get; set; }

        public DetailedException(HttpStatusCode statusCode, ErrorMetadata metadata, string message)
        {
            StatusCode = statusCode;
            ErrorResponse = new ErrorResponse(metadata, message);
        }
    }
}