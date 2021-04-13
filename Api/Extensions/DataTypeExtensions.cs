using System.Collections.Generic;

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
    }
}