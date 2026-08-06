using System;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API;

/// <summary>
/// 请求响应
/// </summary>
/// <typeparam name="T"></typeparam>
public class WsApiResponse<T>
{
    /// <summary>
    /// 状态码
    /// </summary>
    [JsonProperty("status")]
    [JsonConverter(typeof(NormalActionStatusConverter))]
    public Enum_NormalActionStatus  Status { get; set; }
    /// <summary>
    /// 返回码
    /// </summary>
    [JsonProperty("retcode")]
    public int RetCode { get; set; }
    /// <summary>
    /// 数据
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; } = default!;
    /// <summary>
    /// 提示信息
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// 提示信息（人性化）
    /// </summary>
    [JsonProperty("wording")]
    public string Wording { get; set; } = string.Empty;
    /// <summary>
    /// 回显
    /// </summary>
    [JsonProperty("echo")]
    public string Echo { get; set; } = string.Empty;
    /// <summary>
    /// 这个我也不知道是什么
    /// </summary>
    [JsonProperty("stream")]
    public string Stream  { get; set; } = string.Empty;
}

/// <summary>
/// 只实现反序列化, Enum_NormalActionStatus的反序列化
/// </summary>
public class NormalActionStatusConverter : JsonConverter<Enum_NormalActionStatus>
{
    public override void WriteJson(JsonWriter writer, Enum_NormalActionStatus value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Enum_NormalActionStatus ReadJson(
        JsonReader reader,
        Type objectType,
        Enum_NormalActionStatus existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType != JsonToken.String) return Enum_NormalActionStatus.Error;
        var value = reader.Value?.ToString()?.ToLowerInvariant();
        return value switch
        {
            "ok" => Enum_NormalActionStatus.Ok,
            _ => Enum_NormalActionStatus.Error
        };
    }
}