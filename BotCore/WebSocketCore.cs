using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NapcatBot.Core.Models.Core;
using Newtonsoft.Json.Linq;
using Serilog;

namespace NapcatBot.Core.BotCore;

/// <summary>
/// WebSocket(正向)的连接核心
/// </summary>
public class WebSocketCore
{
    /// <summary>
    /// 配置项
    /// </summary>
    public WebsocketOption WebsocketOption { get; set; }

    /// <summary>
    /// ClientWebSocket
    /// </summary>
    public ClientWebSocket WebSocket { get; set; } = null!;

    #region 所有广播事件

    /// <summary>
    /// 连接失败事件
    /// </summary>
    public WSConnectErrorListener? _ConnectErrorListener { get; set; } = null;

    /// <summary>
    /// 连接成功事件
    /// </summary>
    public WSConnectListener? _ConnectListener { get; set; } = null;

    /// <summary>
    /// 连接断开事件
    /// </summary>
    public WSConnectListener? _DisconnectListener { get; set; } = null;

    /// <summary>
    /// 连接关闭
    /// </summary>
    public WSCloseListener? _CloseListener { get; set; } = null;

    /// <summary>
    /// [原始事件] 数据到达处理事件
    /// </summary>
    public WSDataReceivedListener? _DataReceivedListener { get; set; } = null;

    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="option"></param>
    public WebSocketCore(WebsocketOption option)
    {
        WebsocketOption = option;
    }

    /// <summary>
    /// 启动链接
    /// </summary>
    public async Task StartConnection()
    {
        WebSocket = new();
        try
        {
            WebSocket.Options.SetRequestHeader("Authorization", $"Bearer {WebsocketOption.Token}");
            await WebSocket.ConnectAsync(new Uri(WebsocketOption.WsUrl), CancellationToken.None);

            if (_ConnectListener != null)
                await _ConnectListener.Invoke(this);

            _ = Task.Run(async () => await WSDataReceived());
        }
        catch (Exception e)
        {
            if (_ConnectErrorListener != null)
                await _ConnectErrorListener.Invoke(this, e);
            throw;
        }
    }

    /// <summary>
    /// 数据监听事件
    /// </summary>
    protected async Task WSDataReceived()
    {
        var buffer = new byte[WebsocketOption.BufferSize];
        var bufferPool = new List<byte>();

        while (WebSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                bufferPool.AddRange(buffer[..result.Count]);

                if (!result.EndOfMessage) continue;

                // 数据到达事件
                if (_DataReceivedListener != null)
                    await _DataReceivedListener(this, result, bufferPool);
                // 连接已关闭
                if (result.MessageType == WebSocketMessageType.Close && _CloseListener != null)
                {
                    await _CloseListener.Invoke(this);
                    break;
                }
            }
            catch (Exception e)
            {
                Log.Error($"{e}");
            }
            finally
            {
                bufferPool.Clear();
            }
        }
    }
}

/// <summary>
/// 连接失败事件
/// </summary>
public delegate Task WSConnectErrorListener(WebSocketCore core, Exception exception);

/// <summary>
/// WS连接成功事件
/// </summary>
public delegate Task WSConnectListener(WebSocketCore core);

/// <summary>
/// 数据到达事件
/// </summary>
public delegate Task WSDataReceivedListener(WebSocketCore core, WebSocketReceiveResult oriResult, List<byte> buffer);

/// <summary>
/// WS断开事件
/// </summary>
public delegate Task WSDisconnectListener(WebSocketCore core);

/// <summary>
/// WS关闭事件
/// </summary>
public delegate Task WSCloseListener(WebSocketCore core);