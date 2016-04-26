

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Stockfighter.Core.ValueTypes
{

    public static class ValueTypeFactory
    {
          
        public static OrderDirection OrderDirection(string value) => new OrderDirection(value);
          
        public static OrderType OrderType(string value) => new OrderType(value);
          
        public static Symbol Symbol(string value) => new Symbol(value);
          
        public static Price Price(int value) => new Price(value);
          
        public static Quantity Quantity(int value) => new Quantity(value);
    }


    public class ValueAdapterJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
          
            if(value is OrderDirection) { serializer.Serialize(writer,((OrderDirection)value).Value); return; }
          
            if(value is OrderType) { serializer.Serialize(writer,((OrderType)value).Value); return; }
          
            if(value is Symbol) { serializer.Serialize(writer,((Symbol)value).Value); return; }
          
            if(value is Price) { serializer.Serialize(writer,((Price)value).Value); return; }
          
            if(value is Quantity) { serializer.Serialize(writer,((Quantity)value).Value); return; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
           
            if(objectType == typeof(OrderDirection)) { return new OrderDirection(JToken.ReadFrom(reader).Value<string>()); }
          
            if(objectType == typeof(OrderType)) { return new OrderType(JToken.ReadFrom(reader).Value<string>()); }
          
            if(objectType == typeof(Symbol)) { return new Symbol(JToken.ReadFrom(reader).Value<string>()); }
          
            if(objectType == typeof(Price)) { return new Price(JToken.ReadFrom(reader).Value<int>()); }
          
            if(objectType == typeof(Quantity)) { return new Quantity(JToken.ReadFrom(reader).Value<int>()); }
            throw new InvalidOperationException($"Type {objectType} not recognized.");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderDirection) || objectType == typeof(OrderType) || objectType == typeof(Symbol) || objectType == typeof(Price) || objectType == typeof(Quantity);
        }
    }

    [JsonConverter(typeof(ValueAdapterJsonConverter))]
    public partial struct OrderDirection
    {
        public static readonly OrderDirection Buy = new OrderDirection("buy");
        public static readonly OrderDirection Sell = new OrderDirection("sell");
  
        public readonly string Value;


        public OrderDirection(string value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);

        public bool Equals(OrderDirection other) => string.Equals(Value, other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OrderDirection && Equals((OrderDirection) obj);
        }


    }

    [JsonConverter(typeof(ValueAdapterJsonConverter))]
    public partial struct OrderType
    {
        public static readonly OrderType Limit = new OrderType("limit");
        public static readonly OrderType Market = new OrderType("market");
        public static readonly OrderType FillOrKill = new OrderType("fill-or-kill");
        public static readonly OrderType ImmediateOrCancel = new OrderType("immediate-or-cancel");
  
        public readonly string Value;


        public OrderType(string value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);

        public bool Equals(OrderType other) => string.Equals(Value, other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OrderType && Equals((OrderType) obj);
        }


    }

    [JsonConverter(typeof(ValueAdapterJsonConverter))]
    public partial struct Symbol
    {
  
        public readonly string Value;


        public Symbol(string value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);

        public bool Equals(Symbol other) => string.Equals(Value, other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Symbol && Equals((Symbol) obj);
        }


    }

    [JsonConverter(typeof(ValueAdapterJsonConverter))]
    public partial struct Price
    {
  
        public readonly int Value;


        public Price(int value)
        {
            Value = value;
        }

        public override string ToString() => string.Format("{C2}", Value / 100);

        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);

        public bool Equals(Price other) => string.Equals(Value, other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Price && Equals((Price) obj);
        }


        public static Price operator +(Price a, Price b) =>new Price(a.Value + b.Value);
        public static Price operator -(Price a, Price b) =>new Price(a.Value - b.Value);
        public static bool operator >(Price a, Price b) => a.Value > b.Value;
        public static bool operator <(Price a, Price b) => a.Value < b.Value;
        public static bool operator ==(Price a, Price b) => a.Value == b.Value;
        public static bool operator !=(Price a, Price b) => a.Value != b.Value;
    }

    [JsonConverter(typeof(ValueAdapterJsonConverter))]
    public partial struct Quantity
    {
  
        public readonly int Value;


        public Quantity(int value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public override int GetHashCode() => (Value != null ? Value.GetHashCode() : 0);

        public bool Equals(Quantity other) => string.Equals(Value, other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Quantity && Equals((Quantity) obj);
        }


        public static Quantity operator +(Quantity a, Quantity b) =>new Quantity(a.Value + b.Value);
        public static Quantity operator -(Quantity a, Quantity b) =>new Quantity(a.Value - b.Value);
        public static bool operator >(Quantity a, Quantity b) => a.Value > b.Value;
        public static bool operator <(Quantity a, Quantity b) => a.Value < b.Value;
        public static bool operator ==(Quantity a, Quantity b) => a.Value == b.Value;
        public static bool operator !=(Quantity a, Quantity b) => a.Value != b.Value;
    }

}

