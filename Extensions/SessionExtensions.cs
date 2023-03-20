using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyShopPet.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            session.SetString(key, JsonSerializer.Serialize(value, options));
        }
        public static T? Get<T>(this ISession session, string key)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            string? str = session.GetString(key);
            return str != null ? JsonSerializer.Deserialize<T>(str, options) : default;
        }
    }
}
