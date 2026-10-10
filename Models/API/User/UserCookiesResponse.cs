using System.Collections.Generic;
using NapcatBot.Core.Converters;
using Newtonsoft.Json;

namespace NapcatBot.Core.Models.API.User;

/// <summary>
/// 请求地址 /get_cookies
/// </summary>
public class UserCookiesResponse
{
    [JsonProperty("cookies")] 
    [JsonConverter(typeof(CookiesConverter))]
    public Dictionary<string,string> Cookies { get; set; } = [];
    /// <summary>
    /// CSRF Token
    /// </summary>
    [JsonProperty("bkn")] public string Bkn { get; set; } = string.Empty;
}