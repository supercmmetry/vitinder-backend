using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Application.Core
{
    public enum ErrorMetadata
    {
        MissingResource,
        InvalidResource,
        InsufficientAuthorization,
    }
    
    public class ErrorResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorMetadata Metadata;
        
        public string Message;
    }
}