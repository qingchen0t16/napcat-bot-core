using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using NapcatBot.Core.Models.API;
using NapcatBot.Core.Models.API.Group;
using NapcatBot.Core.Models.API.Message;
using NapcatBot.Core.Models.Enum;
using NapcatBot.Core.Utils;
using Newtonsoft.Json.Linq;
using Serilog;

namespace NapcatBot.Core;

public partial class NapcatBot
{
    /// <summary>
    /// 获取全部群聊数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<GroupInfo>> Group_GetAllGroupsAsync()
    {
        var data = await SendAPIMessage<List<GroupInfo>>("get_group_list");
        return data.Data;
    }

    /// <summary>
    /// 获取群组数据
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="noCache"></param>
    /// <returns></returns>
    public async Task<GroupInfo> Group_GetGroupInfoAsync(ulong groupId, bool noCache = false)
    {
        var data = await SendAPIMessage<GroupInfo>("get_group_info", new
        {
            group_id = groupId,
            no_cache = noCache
        });
        return data.Data;
    }

    /// <summary>
    /// 发送群聊消息
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="chain"></param>
    public async Task<WsApiResponse<GroupMessageResponse>> Group_SendMessageAsync(ulong groupId, MsgChain chain)
    {
        return await SendAPIMessage<GroupMessageResponse>("send_group_msg", new GroupMessageQuery
        {
            GroupId = groupId,
            Message = chain
        });
    }

    /// <summary>
    /// 发送群聊消息等待返回
    /// </summary>
    public async Task<MessagePoolTask> Group_SendMessageWaitReplyAsync(ulong groupId, MsgChain chain,
        object? bindData = null,
        int timeout = 30,
        Action<MessagePoolTask>? onTimeout = null)
    {
        // 先发群消息,获取MessageID
        var message = await Group_SendMessageAsync(groupId, chain);
        var task = MessagePoolTask.Create(this, message, bindData, timeout, onTimeout);
        AddMessageTask(task);
        return await task.WaitTask.Task;
    }

    /// <summary>
    /// 设置群请求
    /// </summary>
    /// <param name="flag">ID</param>
    /// <param name="approve">是否同意</param>
    /// <param name="reason">拒绝理由</param>
    /// <returns></returns>
    public async Task<WsApiResponse<object?>> Group_SetGroupRequest(string flag, bool approve, string? reason = "")
    {
        return await SendAPIMessage<object?>("set_group_add_request", new { flag, approve, reason });
    }

    /// <summary>
    /// 群成员是否存在
    /// 不建议用这个, 这个用缓存, 快速判断QQ是否在群内
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Group_ExistMember(ulong groupId, ulong qq, bool noCache = false)
    {
        var ret = await SendAPIMessage<object?>("get_group_member_info", new
        {
            group_id = groupId,
            user_id = qq,
            no_cache = noCache
        });
        return ret.Data != null;
    }

    /// <summary>
    /// 获取群组打卡列表
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public async Task<WsApiResponse<GroupSignedListResponse[]>> Group_GetGroupSignedList(ulong groupId)
    {
        var ret = await SendAPIMessage<GroupSignedListResponse[]>("get_group_signed_list", new
        {
            group_id = groupId,
        });
        return ret;
    }

    /// <summary>
    /// 获取群内用户信息
    /// </summary>
    /// <returns></returns>
    public async Task<WsApiResponse<GroupUserInfoResponse>> Group_GetUserInfo(
        ulong groupId,
        ulong userId,
        bool noCache = true
    )
    {
        var ret = await SendAPIMessage<GroupUserInfoResponse>("get_group_member_info", new
        {
            group_id = groupId,
            user_id = userId,
            no_cache = noCache
        });
        return ret;
    }

    /// <summary>
    /// 获取群打卡列表
    /// 框架没有指定日期的打卡列表, 这个是手动实现的可选日期获取打卡列表
    /// </summary>
    /// <param name="date"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public async Task<List<GroupSignedListResponse>> Group_GetGroupSignedListEx(DateTime date, ulong userID, ulong groupID)
    {
        var userCookies = await User_GetCookies();
        if (userCookies.Status != Enum_NormalActionStatus.Ok)
            throw new("无法获取UserCookies");

        if (!userCookies.Data.Cookies.TryGetValue("p_skey", out var pskey))
            throw new("无法获取UserCookies");
        var gtk = UserTool.GetBknFromSKey(pskey);

        var list = new List<GroupSignedListResponse>();
        int offset = 0, total = 1000;
        while (true)
        {
            if (offset >= total)
                break;

            var response = await new Flurl.Url("https://qun.qq.com/v2/signin/trpc/GetDaySignedList")
                .SetQueryParam("g_tk", gtk)
                .WithCookies(userCookies.Data.Cookies)
                .PostJsonAsync(new
                {
                    dayYmd = date.ToString("yyyyMMdd"),
                    offset,
                    limit = 100,
                    uid = userID.ToString(),
                    groupId = groupID.ToString()
                });
            var responseJObj = await response.GetJsonAsync<JObject>();
            if ((responseJObj["retCode"]?.Value<int>() ?? -1) != 0)
                throw new("无法获取内容");

            var responsePage = ((JArray)responseJObj!["response"]!["page"]!)[0];
            offset = responsePage["offset"]!.Value<int>();
            total = responsePage["total"]!.Value<int>();

            list.AddRange(((JArray?)responsePage["infos"] ?? [])
                .Select(r => new GroupSignedListResponse
                {
                    UserID = r["uid"]!.Value<ulong>(),
                    Nick = r["uidGroupNick"]!.Value<string>()!,
                    Rank = r["signInRank"]!.Value<int>(),
                    Time = r["signedTimeStamp"]!.Value<ulong>(),
                }));
        }

        return list;
    }
}