using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Sharpenter.ResumeParser.OutputFormatter.Json
{
    public class HyphenNameSerializer : JsonConverter
    {
        private static readonly Regex ToHyphenNameRegex = new Regex(@"([a-zA-Z])(?=[A-Z])", RegexOptions.Compiled);
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var valueType = value.GetType();
            if (value is IList)
            {
                writer.WriteStartArray();
                foreach (var instance in (IList)value)
                {
                    serializer.Serialize(writer, instance);
                }
                writer.WriteEndArray();
            }
            else if (valueType.IsPrimitive || valueType == typeof (string))
            {
                writer.WriteValue(value);
            }
            else
            {
                writer.WriteStartObject();

                var properties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    writer.WritePropertyName(ToHyphenNameRegex.Replace(property.Name, "$1-").ToLower());                    
                    if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                    {
                        writer.WriteValue(property.GetValue(value));
                    }
                    else
                    {
                        serializer.Serialize(writer, property.GetValue(value));    
                    }                    
                }

                writer.WriteEndObject();
            }            
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
