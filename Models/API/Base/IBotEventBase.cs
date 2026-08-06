using System;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Base;

public abstract class IBotEventBase
{
    /// <summary>
    /// 时间戳
    /// </summary>
    [JsonProperty("time")]
    public ulong Timestamp { get; set; }

    /// <summary>
    /// 机器人QQ
    /// </summary>
    [JsonProperty("self_id")]
    public ulong SelfId { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [JsonProperty("post_type")]
    [JsonConverter(typeof(BotEventTypeConverter))]
    public Enum_BotEventType PostType { get; set; }
}

/// <summary>
/// 事件类型的Converter
/// </summary>
public class BotEventTypeConverter : JsonConverter<Enum_BotEventType>
{
    public override void WriteJson(JsonWriter writer, Enum_BotEventType value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Enum_BotEventType ReadJson(
        JsonReader reader,
        Type objectType,
        Enum_BotEventType existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return Enum_BotEventType.UnKnown;
        var value = reader.Value?.ToString()?.ToLowerInvariant();
        return value switch
        {
            "meta_event" => Enum_BotEventType.Meta,
            "request" => Enum_BotEventType.Request,
            "notice" => Enum_BotEventType.Notice,
            "message" => Enum_BotEventType.Message,
            "message_sent" => Enum_BotEventType.MessageSent,
            _ => Enum_BotEventType.UnKnown
        };
    }
}