using System;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NapcatBot.Core.Models.API.Group;

/// <summary>
/// 群聊信息发送者消息
/// </summary>
public class GroupSender
{
    /// <summary>
    /// QQ
    /// </summary>
    [JsonProperty("user_id")]
    public ulong UserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonProperty("nickname")]
    public string NickName { get; set; } = string.Empty;

    /// <summary>
    /// 群名片(名称)
    /// </summary>
    [JsonProperty("card")]
    public string CardName { get; set; } = string.Empty;

    /// <summary>
    /// 角色
    /// </summary>
    [JsonProperty("role")]
    [JsonConverter(typeof(GroupSenderRoleConverter))]
    public Enum_GroupSenderRole Role { get; set; }

    /// <summary>
    /// 专属头衔
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 成员等级
    /// </summary>
    [JsonProperty("level")]
    public string Level { get; set; } = string.Empty;
}

public class GroupSenderRoleConverter : JsonConverter<Enum_GroupSenderRole>
{
    public override void WriteJson(JsonWriter writer, Enum_GroupSenderRole value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Enum_GroupSenderRole ReadJson(
        JsonReader reader,
        Type objectType,
        Enum_GroupSenderRole existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return Enum_GroupSenderRole.Unknown;
        var value = reader.Value?.ToString()?.ToLowerInvariant();
        return value switch
        {
            "owner" => Enum_GroupSenderRole.Owner,
            "admin" => Enum_GroupSenderRole.Admin,
            "member" => Enum_GroupSenderRole.Member,
            _ => Enum_GroupSenderRole.Unknown
        };
    }
}