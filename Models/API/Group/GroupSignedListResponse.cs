using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Group;

/// <summary>
/// 群组今日打卡列表
/// </summary>
public class GroupSignedListResponse
{
    [JsonProperty("user_id")]
    public ulong UserID { get; set; }

    [JsonProperty("nick")]
    public string Nick { get; set; } = string.Empty;
    
    [JsonProperty("time")]
    public ulong Time { get; set; }
    
    [JsonProperty("rank")]
    public int Rank { get; set; }
}