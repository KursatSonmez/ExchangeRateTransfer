using Newtonsoft.Json;
using System;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ExchangeRateTransfer.UnitTests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("ExchangeRateTransfer.Worker")]

namespace ExchangeRateTransfer.Common
{
    internal static class JsonExtensions
    {
        internal static string ToJson(this object value, Func<JsonSerializerSettings, JsonSerializerSettings>? settings = null)
        {
            var op = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            op = settings?.Invoke(op) ?? op;

            return JsonConvert.SerializeObject(value, op);
            /*
            var op = new JsonSerializerOptions
            
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            
            //  https://stackoverflow.com/questions/58440400/enum-type-no-longer-working-in-net-core-3-0-frombody-request-object
            // Nullable olmayan enum değerini dönüştürmüyor!
            op.Converters.Add(new JsonStringEnumConverter())

            return JsonSerializer.Serialize(value, op)
            */
        }

        internal static T FromJson<T>(this string value, Func<JsonSerializerSettings, JsonSerializerSettings>? settings = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                return default!;

            var op = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            op = settings?.Invoke(op) ?? op;

            return JsonConvert.DeserializeObject<T>(value, op)!;

            /*if (string.IsNullOrWhiteSpace(value))
                return default

            return JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions
            
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNameCaseInsensitive = propertyNameCaseInsensitive,
            })*/
        }
    }
}
