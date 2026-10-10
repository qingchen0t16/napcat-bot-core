using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NapcatBot.Core.Converters;

public class CookiesConverter : JsonConverter<Dictionary<string,string>>
{
    public override void WriteJson(JsonWriter writer,Dictionary<string,string>? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Dictionary<string,string> ReadJson(
        JsonReader reader,
        Type objectType,
        Dictionary<string,string>? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return [];
        var value = reader.Value?.ToString();
        if (string.IsNullOrEmpty(value))
            return [];
        
        return value.Split(";")
            .Select(r => r.Trim().Split("="))
            .Where(r => r.Length == 2)
            .ToDictionary(r => r[0], r => r[1]);
    }
}