using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API;

/// <summary>
/// Websocket API请求的基类
/// </summary>
public class WsApiRequestBase
{
    /// <summary>
    /// 请求API
    /// </summary>
    [JsonProperty("action")]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// 请求数据
    /// </summary>
    [JsonProperty("params")]
    public object? Params { get; set; } = null;

    /// <summary>
    /// 回显,用来当标识符
    /// </summary>
    [JsonProperty("echo")]
    public string? Echo { get; set; } = string.Empty;
}