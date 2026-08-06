using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NapcatBot.Core.Models.API;
using NapcatBot.Core.Models.API.Group;
using NapcatBot.Core.Models.API.Message;

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
}