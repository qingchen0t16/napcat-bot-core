namespace NapcatBot.Core.Models.Enum;

/// <summary>
/// 通知类型
/// </summary>
public enum Enum_NoticeType
{
    /// <summary>
    /// 群成员增加
    /// </summary>
    GroupIncrease,

    /// <summary>
    /// 未知
    /// </summary>
    Unknown
}

/// <summary>
/// 请求类型
/// </summary>
public enum Enum_RequestType
{
    /// <summary>
    /// 好友请求
    /// </summary>
    Friend,

    /// <summary>
    /// 群请求
    /// </summary>
    Group,

    /// <summary>
    /// 未知
    /// </summary>
    Unknown
}

/// <summary>
/// 群成员增加类型
/// </summary>
public enum Enum_GroupIncreaseType
{
    /// <summary>
    /// 同意进群
    /// </summary>
    Approve,

    /// <summary>
    /// 邀请进群
    /// </summary>
    Invite,

    /// <summary>
    /// 未知
    /// </summary>
    Unknown
}

public enum Enum_GroupDecreaseType
{
    /// <summary>
    /// 主动退群
    /// </summary>
    Leave,

    /// <summary>
    /// 成员被踢
    /// </summary>
    Kick,

    /// <summary>
    /// 机器人被踢
    /// </summary>
    KickMe,

    /// <summary>
    /// 未知
    /// </summary>
    Unknown
}