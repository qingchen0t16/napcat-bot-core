using System;
using System.Threading.Tasks;
using NapcatBot.Core.BotCore;

namespace NapcatBot.Core.Interface;

/// <summary>
/// Websocket的基础接口
/// </summary>
public interface IWebsocketHandle : IHandleBase
{
    /// <summary>
    /// 连接成功
    /// </summary>
    /// <param name="core"></param>
    public Task HandleConnectAsync(WebSocketCore core);

    /// <summary>
    /// 连接失败
    /// </summary>
    /// <param name="core"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    public Task HandleConnectErrorAsync(WebSocketCore core, Exception ex);

    /// <summary>
    /// 连接正常关闭
    /// </summary>
    /// <param name="core"></param>
    /// <returns></returns>
    public Task HandleCloseAsync(WebSocketCore core);
}