using System;
using System.Threading;
using System.Threading.Tasks;
using NapcatBot.Core.Models.API.Group;

namespace NapcatBot.Core.Models.API.Message;

/// <summary>
/// 发送的消息记录任务
/// 主要用于碰到需要多长多长时间内回复本消息的情况
/// </summary>
public class MessagePoolTask
{
    /// <summary>
    /// 创建Task
    /// </summary>
    /// <returns></returns>
    public static MessagePoolTask Create(NapcatBot bot, WsApiResponse<GroupMessageResponse> message,
        object? bindData = null, int timeout = 30,
        Action<MessagePoolTask>? onTimeout = null)
    {
        var task = new MessagePoolTask
        {
            Bot = bot,
            GroupMessageData = message,
            MessageId = message.Data.MessageId.ToString(),
            BindData = bindData,
            Timeout = timeout,
            OnTimeout = onTimeout
        };
        // 开始计算超时
        task.StartTimeout();
        return task;
    }

    /// <summary>
    /// 机器人实例
    /// </summary>
    public NapcatBot Bot { get; set; } = null!;

    /// <summary>
    /// 消息ID
    /// </summary>
    public string MessageId { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 超时时间, 单位秒
    /// 默认30秒
    /// </summary>
    public int Timeout { get; set; } = 30;

    /// <summary>
    /// 传递数据
    /// </summary>
    public object? BindData { get; set; }

    /// <summary>
    /// 返回数据
    /// </summary>
    public MsgChain? ReturnData { get; set; }

    /// <summary>
    /// 消息数据
    /// </summary>
    public WsApiResponse<GroupMessageResponse> GroupMessageData { get; set; } = null!;

    /// <summary>
    /// Task
    /// </summary>
    public TaskCompletionSource<MessagePoolTask> WaitTask { get; set; } = new();

    /// <summary>
    /// 超时遗嘱
    /// </summary>
    public Action<MessagePoolTask>? OnTimeout { get; set; }

    /// <summary>
    /// 超时管理
    /// </summary>
    private CancellationTokenSource? _TimeoutCts { get; set; }

    /// <summary>
    /// 启动超时
    /// </summary>
    private void StartTimeout()
    {
        _TimeoutCts = new CancellationTokenSource();
        _ = WatchTimeoutAsync(_TimeoutCts.Token);
    }

    /// <summary>
    /// 超时方法
    /// </summary>
    /// <param name="ct"></param>
    private async Task WatchTimeoutAsync(CancellationToken ct)
    {
        await Task.Delay(TimeSpan.FromSeconds(Timeout), ct);
        OnTimeout?.Invoke(this);
        WaitTask.TrySetResult(this);
        Bot.RemoveMessageTask(this);
        WaitTask.SetException(new Exception("Timeout, 超时"));
    }
}