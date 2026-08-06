using System;
using NapcatBot.Core.Models.API.Base;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NapcatBot.Core.Models.API.Group;

/// <summary>
/// 群成员增加通知
/// </summary>
public class GroupIncreaseEvent : IBotNoticeEvent
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
    [JsonConverter(typeof(GroupIncreaseTypeConverter))]
    public Enum_GroupIncreaseType SubType { get; set; }
}

public class GroupIncreaseTypeConverter : JsonConverter<Enum_GroupIncreaseType>
{
    public override void WriteJson(JsonWriter writer, Enum_GroupIncreaseType value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Enum_GroupIncreaseType ReadJson(
        JsonReader reader,
        Type objectType,
        Enum_GroupIncreaseType existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return Enum_GroupIncreaseType.Unknown;
        var value = reader.Value?.ToString()?.ToLowerInvariant();
        return value switch
        {
            "approve" => Enum_GroupIncreaseType.Approve,
            "invite" => Enum_GroupIncreaseType.Invite,
            _ => Enum_GroupIncreaseType.Unknown
        };
    }
}