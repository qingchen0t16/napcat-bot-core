using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Group;

/// <summary>
/// 群信息
/// </summary>
public class GroupInfo
{
    /// <summary>
    /// 群号
    /// </summary>
    [JsonProperty("group_id")]
    public ulong GroupId { get; set; }

    /// <summary>
    /// 群名称
    /// </summary>
    [JsonProperty("group_name")]
    public string GroupName { get; set; } = string.Empty;

    /// <summary>
    /// 群成员数
    /// </summary>
    [JsonProperty("member_count")]
    public int MemberCount { get; set; }

    /// <summary>
    /// 最大成员数(群容量)
    /// </summary>
    [JsonProperty("max_member_count")]
    public int MaxMemberCount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [JsonProperty("group_remark")]
    public string GroupRemark { get; set; } = string.Empty;

    /// <summary>
    /// 这个是什么 我也不知道
    /// </summary>
    [JsonProperty("group_all_shut")]
    public int GroupAllShut { get; set; }
}