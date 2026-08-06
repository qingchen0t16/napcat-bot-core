using System.Threading.Tasks;
using NapcatBot.Core.Models.API.Base;
using NapcatBot.Core.Models.API.Group;
using NapcatBot.Core.Models.API.Message;

namespace NapcatBot.Core.Interface;

/// <summary>
/// 群消息事件
/// </summary>
public interface IWebsocketGroupHandle : IHandleBase
{
    /// <summary>
    /// 群消息到达
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    public Task HandleGroupMessage(NapcatBot bot, GroupMessage message, MsgChain chain);
}

/// <summary>
/// 群请求事件
/// </summary>
public interface IWebsocketGroupRequestHandle : IHandleBase
{
    /// <summary>
    /// 群请求
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    public Task HandleGroupRequest(NapcatBot bot, GroupRequest request);
}

/// <summary>
/// 群通知事件
/// </summary>
public interface IWebsocketGroupNoticeHandle : IHandleBase
{
    /// <summary>
    /// 群成员增加通知
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public Task HandleGroupIncrease(NapcatBot bot, GroupIncreaseEvent e);

    /// <summary>
    /// 群成员减少通知
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public Task HandleGroupDecrease(NapcatBot bot, GroupDecreaseEvent e);
}