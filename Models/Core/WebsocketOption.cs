namespace NapcatBot.Core.Models.Core;

/// <summary>
/// WS的配置项
/// </summary>
public class WebsocketOption
{
    /// <summary>
    /// WS的请求地址
    /// </summary>
    public string WsUrl { get; set; } = string.Empty;
    /// <summary>
    /// WS请求鉴权
    /// </summary>
    public string Token { get; set; } = string.Empty;
    /// <summary>
    /// 接受池大小
    /// </summary>
    public int BufferSize { get; set; } = 1024 * 2;
}