using System;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Base;

/// <summary>
/// 群组通知事件基类
/// 处理群相关的通知事件
/// </summary>
public class IGroupNoticeEvent : IBotEventBase
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonProperty("group_id")]
    public ulong GroupId { get; set; }

    /// <summary>
    /// 用户 QQ 号
    /// </summary>
    [JsonProperty("user_id")]
    public ulong UserQQ { get; set; }
}

/// <summary>
/// 通知基类
/// </summary>
public class IBotNoticeEvent : IGroupNoticeEvent
{
    /// <summary>
    /// 通知类型
    /// </summary>
    [JsonProperty("notice_type")]
    [JsonConverter(typeof(BotNoticeTypeConverter))]
    public Enum_NoticeType NoticeType { get; set; }
}

/// <summary>
/// 通知类型的Converter
/// </summary>
public class BotNoticeTypeConverter : JsonConverter<Enum_NoticeType>
{
    public override void WriteJson(JsonWriter writer, Enum_NoticeType value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Enum_NoticeType ReadJson(
        JsonReader reader,
        Type objectType,
        Enum_NoticeType existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return Enum_NoticeType.Unknown;
        var value = reader.Value?.ToString()?.ToLowerInvariant();
        return value switch
        {
            "group_increase" => Enum_NoticeType.GroupIncrease,
            _ => Enum_NoticeType.Unknown
        };
    }
}