using System;
using NapcatBot.Core.Models.API.Group;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Base;

/// <summary>
/// 群组请求事件基类
/// </summary>
public class IGroupRequestEvent : IGroupNoticeEvent
{
    /// <summary>
    /// 群请求
    /// </summary>
    [JsonProperty("request_type")]
    [JsonConverter(typeof(GroupRequestTypeConverter))]
    public Enum_RequestType RequestType { get; set; }
}

public class GroupRequestTypeConverter : JsonConverter<Enum_RequestType>
{
    public override void WriteJson(JsonWriter writer, Enum_RequestType value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Enum_RequestType ReadJson(
        JsonReader reader,
        Type objectType,
        Enum_RequestType existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return Enum_RequestType.Unknown;
        var value = reader.Value?.ToString()?.ToLowerInvariant();
        return value switch
        {
            "friend" => Enum_RequestType.Friend,
            "group" => Enum_RequestType.Group,
            _ => Enum_RequestType.Unknown
        };
    }
}