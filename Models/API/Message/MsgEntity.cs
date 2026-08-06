using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Message;

/// <summary>
/// 消息链实体接口
/// </summary>
public interface IMsgEntity
{
}

/// <summary>
/// 消息链实体
/// </summary>
/// <typeparam name="T"></typeparam>
public class MsgEntity<T> : IMsgEntity
{
    /// <summary>
    /// 快速创建实体
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static MsgEntity<T> Create(T entity)
    {
        var ret = new MsgEntity<T>();
        ret.Type = entity switch
        {
            MsgTextEntity => "text",
            MsgAtEntity => "at",
            MsgReplyEntity => "reply",
            MsgImageEntity => "image",
            _ => "none"
        };
        ret.Data = entity;
        return ret;
    }

    /// <summary>
    /// 类型
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 数据
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; } = default!;
    
    public override string ToString()
    {
        return Data?.ToString() ?? "Null";
    }
}

/// <summary>
/// 文本实体
/// </summary>
public class MsgTextEntity
{
    /// <summary>
    /// 文本消息
    /// </summary>
    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;

    public override string ToString()
    {
        return Text;
    }
}

/// <summary>
/// At实体
/// </summary>
public class MsgAtEntity
{
    /// <summary>
    /// AT全体成员
    /// </summary>
    /// <returns></returns>
    public static MsgAtEntity AtAll()
    {
        return new MsgAtEntity
        {
            QQ = "all"
        };
    }

    /// <summary>
    /// QQ号
    /// </summary>
    [JsonProperty("qq")]
    public string QQ { get; set; } = string.Empty;
}

/// <summary>
/// 回复
/// </summary>
public class MsgReplyEntity
{
    /// <summary>
    /// 欲回复MessageID
    /// </summary>
    [JsonProperty("id")]
    public string MsgId { get; set; } = string.Empty;
}

/// <summary>
/// 图片
/// </summary>
public class MsgImageEntity
{
    [JsonProperty("file")]
    public string FilePath { get; set; } = string.Empty;
}