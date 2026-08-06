namespace NapcatBot.Core.Models.Enum;

/// <summary>
/// 群角色
/// </summary>
public enum Enum_GroupSenderRole
{
    /// <summary>
    /// 作者
    /// </summary>
    Owner,

    /// <summary>
    /// 管理员
    /// </summary>
    Admin,

    /// <summary>
    /// 群员
    /// </summary>
    Member,

    /// <summary>
    /// 未知
    /// </summary>
    Unknown
}

/// <summary>
/// 消息类型
/// </summary>
public enum Enum_MessageType
{
    /// <summary>
    /// 未知
    /// </summary>
    Unknown,

    /// <summary>
    /// 群消息
    /// </summary>
    Group,

    /// <summary>
    /// 私聊消息
    /// </summary>
    Private
}