using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinChatter.Application.Extensions
{
    public static class StringExtender
    {
        public static bool IsJson(this string source)
        {
            if (source == null)
                return false;

            try
            {
                JsonDocument.Parse(source);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public static T FromJson<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
