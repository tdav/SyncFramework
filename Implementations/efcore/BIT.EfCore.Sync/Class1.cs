using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BIT.EfCore.Sync
{
    public class KeysJsonConverter : JsonConverter
    {
        private readonly Type[] _types;

        public KeysJsonConverter(params Type[] types)
        {
            _types = types;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type myType = value.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            var CurrentModification = value as ColumnModification;


            var prope = CurrentModification.Property as Property;

          
            JToken x2 = JToken.FromObject(prope.PropertyInfo);
            ColumnModification columnModification = new ColumnModification(CurrentModification.ColumnName, CurrentModification.OriginalValue, CurrentModification.Value, CurrentModification.Property, CurrentModification.IsRead, CurrentModification.IsWrite, CurrentModification.IsKey, CurrentModification.IsCondition, true);
            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(value, null);
                JToken x = JToken.FromObject(propValue);
                // Do something with propValue
            }
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

                o.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return _types.Any(t => t == objectType);
        }
    }
}

