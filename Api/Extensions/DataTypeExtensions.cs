using System.Collections.Generic;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Api.Extensions
{
    public static class DataTypeExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue value = default(TValue);
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public static string GetUserId(this HttpContext context)
        {
            return context.Items.GetOrDefault("UserId") as string;
        }

        public static User GetUser(this HttpContext context)
        {
            return context.Items.GetOrDefault("User") as User;
        }

        public static IFormFile GetValidatedFile(this HttpContext context, string name)
        {
            return (context.Items["ValidatedFiles"] as IFormFileCollection)?.GetFile(name);
        }
    }
}