using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace SharedServices.UI.Extensions
{
    public static class SessionExtensions
    {
        public static void InsertValue<T>(this ISession session, string name, T value)
        {
            session.SetString(name, JsonSerializer.Serialize(value));
        }

        public static T GetValue<T>(this ISession session, string name)
        {
            var value = session.GetString(name);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}