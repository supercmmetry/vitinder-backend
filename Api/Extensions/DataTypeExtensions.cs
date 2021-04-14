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

        public static string GetFirebaseUserId(this HttpContext context)
        {
            return context.Items.GetOrDefault("FirebaseUserId") as string;
        }

        public static User GetCurrentUser(this HttpContext context)
        {
            return context.Items.GetOrDefault("CurrentUser") as User;
        }
    }
}