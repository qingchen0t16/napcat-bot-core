namespace NapcatBot.Core.Models.Enum;

/// <summary>
/// API请求返回状态
/// </summary>
public enum Enum_NormalActionStatus
{
    Ok,
    Error
}

/// <summary>
/// 事件枚举类型
/// </summary>
public enum Enum_BotEventType
{
    /// <summary>
    /// 元事件
    /// </summary>
    Meta,

    /// <summary>
    /// 请求事件
    /// </summary>
    Request,

    /// <summary>
    /// 通知事件
    /// </summary>
    Notice,

    /// <summary>
    /// 消息事件
    /// </summary>
    Message,

    /// <summary>
    /// 消息发送事件
    /// </summary>
    MessageSent,

    /// <summary>
    /// 未知事件
    /// </summary>
    UnKnown
}