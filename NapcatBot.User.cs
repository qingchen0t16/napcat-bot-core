using System.Threading.Tasks;
using NapcatBot.Core.Models.API;
using NapcatBot.Core.Models.API.User;

namespace NapcatBot.Core;

public partial class NapcatBot
{
    /// <summary>
    /// 获取Cookies
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public async Task<WsApiResponse<UserCookiesResponse>> User_GetCookies(
        string domain = "qun.qq.com"
    )
    {
        var ret = await SendAPIMessage<UserCookiesResponse>("get_cookies", new
        {
            domain
        });
        return ret;
    }
}
