using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.Group;

public class GroupUserInfoResponse
{
    [JsonProperty("group_id")] public ulong GroupID { get; set; }

    [JsonProperty("user_id")] public ulong UserID { get; set; }

    [JsonProperty("nickname")] public string NickName { get; set; } = string.Empty;

    [JsonProperty("card")] public string Card { get; set; } = string.Empty;

    [JsonProperty("sex")] public string Sex { get; set; } = string.Empty;

    [JsonProperty("age")] public int Age { get; set; }

    [JsonProperty("join_time")] public ulong JoinTime { get; set; }

    [JsonProperty("last_sent_time")] public ulong LastSentTime { get; set; }

    [JsonProperty("level")] public string Level { get; set; } = string.Empty;

    [JsonProperty("qq_level")] public int QQLevel { get; set; }

    [JsonProperty("role")] public string Role { get; set; } = string.Empty;

    [JsonProperty("title")] public string Title { get; set; } = string.Empty;

    [JsonProperty("area")] public string Area { get; set; } = string.Empty;

    [JsonProperty("unfriendly")] public bool Unfriendly { get; set; }

    [JsonProperty("title_expire_time")] public ulong TitleExpireTime { get; set; }

    [JsonProperty("card_changeable")] public bool CardChangeable { get; set; }

    [JsonProperty("shut_up_timestamp")] public ulong ShutUpTimestamp { get; set; }

    [JsonProperty("is_robot")] public bool IsRobot { get; set; }

    [JsonProperty("qage")] public int QAge { get; set; }
}