using System;
using NapcatBot.Core.Models.API.Base;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Group;

/// <summary>
/// 群成员减少
/// </summary>
public class GroupDecreaseEvent : IBotNoticeEvent
{
    /// <summary>
    /// 操作者 QQ 号
    /// </summary>
    [JsonProperty("operator_id")]
    public string OperatorId { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型
    /// </summary>
    [JsonProperty("sub_type")]
    [JsonConverter(typeof(GroupDecreaseTypeConverter))]
    public Enum_GroupDecreaseType SubType { get; set; }
}

public class GroupDecreaseTypeConverter : JsonConverter<Enum_GroupDecreaseType>
{
    public override void WriteJson(JsonWriter writer, Enum_GroupDecreaseType value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Enum_GroupDecreaseType ReadJson(
        JsonReader reader,
        Type objectType,
        Enum_GroupDecreaseType existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return Enum_GroupDecreaseType.Unknown;
        var value = reader.Value?.ToString()?.ToLowerInvariant();
        return value switch
        {
            "leave" => Enum_GroupDecreaseType.Leave,
            "kick" => Enum_GroupDecreaseType.Kick,
            "kick_me" => Enum_GroupDecreaseType.KickMe,
            _ => Enum_GroupDecreaseType.Unknown
        };
    }
}