using System.Text.Json.Serialization;

namespace LawfulBlade.Core
{
    public struct InstanceVariable
    {
        [JsonInclude]
        public string Root;

        [JsonInclude]
        public string Key;

        [JsonInclude]
        public string Value;

        [JsonInclude, JsonConverter(typeof(JsonStringEnumConverter))]
        public InstanceVariableType Type;     // "Integer" (REG_DWORD), "String" (REG_SZ), "Binary" (REG_BINARY)

        /// <summary>
        /// Gets the value of the variable according to the type
        /// </summary>
        public object GetValue()
        {
            return Type switch 
            {
                InstanceVariableType.Integer => long.Parse(Value),
                InstanceVariableType.String  => Value,
                InstanceVariableType.Binary  => Convert.FromBase64String(Value),

                // How about you don't fucking use non enum values?..
                _ => throw new Exception("Unimplemented Variable Type!")
            };
        }

        /// <summary>
        /// Sets the value according to a given type
        /// </summary>
        public void SetValue(object value, InstanceVariableType type = InstanceVariableType.Integer)
        {
            // Set the new type...
            Type = type;

            // Set the value...
            Value = Type switch
            {
                InstanceVariableType.Integer => $"{(long)value}",
                InstanceVariableType.String  => (string)value,
                InstanceVariableType.Binary  => Convert.ToBase64String((byte[])value),

                // How about you don't fucking use non enum values?..
                _ => throw new Exception("Unimplemented Variable Type!")
            };
        }
    }

    public enum InstanceVariableType
    {
        Integer,
        String,
        Binary
    }
}
