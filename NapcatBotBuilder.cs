using System;
using NapcatBot.Core.BotCore;
using NapcatBot.Core.Interface;
using NapcatBot.Core.Models.Core;

namespace NapcatBot.Core;

/// <summary>
/// Bot实体的构建类
/// </summary>
public class NapcatBotBuilder : NapcatBot
{
    public static NapcatBotBuilder Create()
    {
        return new NapcatBotBuilder();
    }

    /// <summary>
    /// 启用WS
    /// </summary>
    /// <returns></returns>
    public NapcatBotBuilder UseWebsocket(WebsocketOption option)
    {
        WebSocket = new(option);

        return this;
    }

    /// <summary>
    /// 自调用
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    public NapcatBotBuilder Use(Action<NapcatBotBuilder> func)
    {
        func(this);
        return this;
    }

    /// <summary>
    /// 挂载Plugin
    /// </summary>
    /// <returns></returns>
    public NapcatBotBuilder UsePlugin<T>() where T : IHandleBase, new()
    {
        var plugin = new T();
        if (plugin is IWebsocketHandle websocketHandle)
            BindWebsocketHandle(websocketHandle);
        if (plugin is IWebsocketGroupHandle groupHandle)
            _GroupMessageListener += groupHandle.HandleGroupMessage;
        if (plugin is IWebsocketGroupRequestHandle requestHandle)
            _GroupRequestListener += requestHandle.HandleGroupRequest;
        if (plugin is IWebsocketGroupNoticeHandle noticeHandle)
        {
            _GroupIncreaseListener += noticeHandle.HandleGroupIncrease;
            _GroupDecreaseListener += noticeHandle.HandleGroupDecrease;
        }

        return this;
    }

    /// <summary>
    /// IWebsocketHandle事件
    /// </summary>
    /// <param name="plugin"></param>
    /// <returns></returns>
    protected NapcatBotBuilder BindWebsocketHandle(IWebsocketHandle plugin)
    {
        WebSocket!._ConnectListener += plugin.HandleConnectAsync;
        WebSocket!._ConnectErrorListener += plugin.HandleConnectErrorAsync;
        WebSocket!._CloseListener += plugin.HandleCloseAsync;

        return this;
    }


    /// <summary>
    /// 构建, 就相当于初始化了喔
    /// </summary>
    /// <returns></returns>
    public NapcatBot Build()
    {
        return this;
    }
}