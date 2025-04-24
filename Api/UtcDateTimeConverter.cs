using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api
{
    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-ddTHH:mm:ssZ";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();

            // Si la fecha tiene milisegundos, los eliminamos
            if (str.Contains("."))
            {
                str = str.Substring(0, str.LastIndexOf("."));
            }

            // Parseamos la fecha sin milisegundos
            return DateTime.ParseExact(str, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Formateamos la fecha sin milisegundos y la convertimos a UTC
            writer.WriteStringValue(value.ToUniversalTime().ToString(Format));
        }
    }

}
