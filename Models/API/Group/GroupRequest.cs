using NapcatBot.Core.Models.API.Base;
using NapcatBot.Core.Models.Enum;
using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Group;

/// <summary>
/// 群请求事件
/// </summary>
public class GroupRequest : IGroupRequestEvent
{
    /// <summary>
    /// 验证信息
    /// </summary>
    [JsonProperty("comment")]
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// 请求标识
    /// </summary>
    [JsonProperty("flag")]
    public string Flag { get; set; } = string.Empty;

    /// <summary>
    /// 请求子类型
    /// </summary>
    [JsonProperty("sub_type")]
    public string SubType { get; set; } = string.Empty;
}