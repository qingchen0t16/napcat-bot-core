using System;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NapcatBot.Core.Models.API.Base;

/// <summary>
/// 消息事件 (Message Event)
/// 消息事件用于接收各类消息，包括私聊和群聊消息
/// </summary>
public abstract class IBaseMessageEvent : IBotEventBase
{
    /// <summary>
    /// 消息ID
    /// </summary>
    [JsonProperty("message_id")]
    public ulong MessageId { get; set; }

    /// <summary>
    /// 发消息的QQ
    /// </summary>
    [JsonProperty("user_id")]
    public ulong UserId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [JsonProperty("message")]
    public JArray MessageList { get; set; } = [];

    /// <summary>
    /// 原始信息(就是个文字版的MessageList)
    /// </summary>
    [JsonProperty("raw_message")]
    public string RawMessage { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型
    /// </summary>
    [JsonProperty("message_type")]
    [JsonConverter(typeof(MessageTypeConverter))]
    public Enum_MessageType MessageType { get; set; } = Enum_MessageType.Unknown;
}

public class MessageTypeConverter : JsonConverter<Enum_MessageType>
{
    public override void WriteJson(JsonWriter writer, Enum_MessageType value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Enum_MessageType ReadJson(
        JsonReader reader,
        Type objectType,
        Enum_MessageType existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return Enum_MessageType.Unknown;
        var value = reader.Value?.ToString()?.ToLowerInvariant();
        return value switch
        {
            "private" => Enum_MessageType.Private,
            "group" => Enum_MessageType.Group,
            _ => Enum_MessageType.Unknown
        };
    }
}