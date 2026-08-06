using NapcatBot.Core.Models.API.Base;
using NapcatBot.Core.Models.API.Message;
using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Group;

/// <summary>
/// 群聊数据
/// </summary>
public class GroupMessage : IBaseMessageEvent
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonProperty("group_id")]
    public ulong GroupId { get; set; }

    /// <summary>
    /// 匿名信息
    /// </summary>
    [JsonProperty("anonymous")]
    public object? Anonymous { get; set; }

    /// <summary>
    /// 发送者信息
    /// </summary>
    [JsonProperty("sender")]
    public GroupSender Sender { get; set; } = null!;

    /// <summary>
    /// 创建回复消息链
    /// </summary>
    /// <returns></returns>
    public MsgChain CreateReplyMsgChain()
    {
        return new MsgChain().Reply(MessageId);
    }
}

/// <summary>
/// 发送群数据请求体
/// </summary>
public class GroupMessageQuery
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonProperty("group_id")]
    public ulong GroupId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [JsonProperty("message")]
    public object Message { get; set; } = null!;
}

/// <summary>
/// 群消息发送完的返回值
/// </summary>
public class GroupMessageResponse
{
    /// <summary>
    /// 消息ID
    /// </summary>
    [JsonProperty("message_id")]
    public ulong MessageId { get; set; }
}