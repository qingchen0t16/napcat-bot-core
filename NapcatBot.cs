using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NapcatBot.Core.BotCore;
using NapcatBot.Core.Models.API;
using NapcatBot.Core.Models.API.Group;
using NapcatBot.Core.Models.API.Message;
using NapcatBot.Core.Models.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Events;

namespace NapcatBot.Core;

public partial class NapcatBot
{
    /// <summary>
    /// Websocket部分核心
    /// </summary>
    public WebSocketCore? WebSocket { get; protected set; }

    /// <summary>
    /// 接收Task队列
    /// </summary>
    public Dictionary<string, WebsocketTask> TaskList { get; set; } = new();

    /// <summary>
    /// 等待返回消息Task队列
    /// </summary>
    public Dictionary<string, MessagePoolTask> MessageTaskList { get; set; } = new();

    #region 各类Bot的监听事件

    public GroupMessageListener? _GroupMessageListener { get; set; }
    public GroupRequestListener? _GroupRequestListener { get; set; }
    public GroupIncreaseListener? _GroupIncreaseListener { get; set; }
    public GroupDecreaseListener? _GroupDecreaseListener { get; set; }

    #endregion

    /// <summary>
    /// 启动机器人实例
    /// </summary>
    public async Task StartAsync()
    {
        if (WebSocket is null)
            return;

        // 初始化Log
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            // 对于控制台Sink，只允许Info及以上级别的日志
#if DEBUG
            .WriteTo.Console(LogEventLevel.Debug)
#else
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
#endif
            .WriteTo.File("Logs/Log-.txt", rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .CreateLogger();

        WebSocket._DataReceivedListener += HandelDataReceived;
        await WebSocket.StartConnection();
    }

    /// <summary>
    /// 自实现一个数据到达解析
    /// </summary>
    protected async Task HandelDataReceived(
        WebSocketCore core,
        WebSocketReceiveResult oriResult,
        List<byte> buffer
    )
    {
        // 文本数据
        if (oriResult.MessageType == WebSocketMessageType.Text)
        {
            var data = Encoding.UTF8.GetString(buffer.ToArray(), 0, oriResult.Count);
            Log.Information($"收到数据: {data}");

            var jObj = JObject.Parse(data);

            // 判断是不是是API返回值
            var echo = jObj["echo"]?.Value<string>();
            if (echo != null)
            {
                if (!TaskList.TryGetValue(echo, out var task))
                    return;
                task.EndTime = DateTime.UtcNow;
                Log.Information($"{echo}耗时: {(task.EndTime - task.StartTime).TotalSeconds} s");
                task.Task.TrySetResult(jObj);
            }

            // 开始处理事件类型
            var postType = jObj["post_type"]?.Value<string>();
            switch (postType)
            {
                case null:
                    return;
                // 消息事件
                case "message":
                {
                    var messageType = jObj["message_type"]?.Value<string>();
                    // 消息列表
                    var msgList = jObj["message"]?.Value<JArray>() ?? [];
                    var msgChain = MsgChain.Parse(msgList);
                    Console.WriteLine(JsonConvert.SerializeObject(msgChain));
                    // 回复消息部分逻辑
                    if (msgChain.TryGetReplyMsgID(out var msgId) && MessageTaskList.TryGetValue(msgId!, out var task))
                    {
                        // 是存在需要返回的
                        task.ReturnData = msgChain;
                        task.WaitTask.TrySetResult(task);
                        return;
                    }

                    // 处理消息
                    switch (messageType)
                    {
                        case null:
                            return;
                        // 群聊
                        case "group":
                            // 群消息
                            var message = jObj.ToObject<GroupMessage>();
                            if (message is null) return;

                            _GroupMessageListener?.Invoke(this, message, msgChain);
                            break;
                    }

                    break;
                }
                // 通知事件
                case "notice":
                    var noticeType = jObj["notice_type"]?.Value<string>();
                    switch (noticeType)
                    {
                        case null:
                            return;
                        // 群成员增加
                        case "group_increase":
                            var groupIncrease = jObj.ToObject<GroupIncreaseEvent>();
                            if (groupIncrease is null) return;
                            _GroupIncreaseListener?.Invoke(this, groupIncrease);
                            break;
                        // 群成员减少
                        case "group_decrease":
                            var groupDecrease = jObj.ToObject<GroupDecreaseEvent>();
                            if (groupDecrease is null) return;
                            _GroupDecreaseListener?.Invoke(this, groupDecrease);
                            break;
                    }

                    break;
                // 请求事件
                case "request":
                    var requestType = jObj["request_type"]?.Value<string>();
                    switch (requestType)
                    {
                        case null:
                            return;
                        // 群请求
                        case "group":
                            var groupRequest = jObj.ToObject<GroupRequest>();
                            if (groupRequest is null) return;
                            _GroupRequestListener?.Invoke(this, groupRequest);
                            break;
                    }
                    break;
            }
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// 发送API消息
    /// </summary>
    /// <param name="data"></param>
    /// <param name="messageID"></param>
    /// <typeparam name="T"></typeparam>
    protected async Task<WsApiResponse<T>> SendAPIMessage<T>(string action, object? data = null)
    {
        var sendData = new WsApiRequestBase
        {
            Action = action,
            Echo = Guid.NewGuid().ToString(),
            Params = data
        };
        var sendDataJson = JsonConvert.SerializeObject(sendData);
        Log.Information($"调用API: {sendDataJson}");

        // 添加到处理队列
        var queue = new WebsocketTask
        {
            SendData = sendData,
            TaskID = sendData.Echo
        };
        TaskList.Add(queue.TaskID, queue);
        // 发送数据
        await WebSocket!.WebSocket.SendAsync(
            new ArraySegment<byte>(Encoding.Default.GetBytes(sendDataJson)),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
        return (await queue.Task.Task).ToObject<WsApiResponse<T>>() ?? throw new("无法获取数据");
    }

    /// <summary>
    /// 添加MessageTask
    /// </summary>
    /// <param name="task"></param>
    public void AddMessageTask(MessagePoolTask task)
    {
        if (MessageTaskList.TryAdd(task.MessageId, task)) return;
        // 更新任务,只会保留最新一个task数据
        MessageTaskList[task.MessageId] = task;
    }

    /// <summary>
    /// 移除MessageTask
    /// </summary>
    /// <param name="task"></param>
    public void RemoveMessageTask(MessagePoolTask task)
    {
        MessageTaskList.Remove(task.MessageId);
    }
}

/// <summary>
/// 群消息事件
/// </summary>
public delegate Task GroupMessageListener(NapcatBot bot, GroupMessage message, MsgChain chain);
/// <summary>
/// 群请求事件
/// </summary>
public delegate Task GroupRequestListener(NapcatBot bot, GroupRequest request);
/// <summary>
/// 群成员增加事件
/// </summary>
public delegate Task GroupIncreaseListener(NapcatBot bot, GroupIncreaseEvent e);
/// <summary>
/// 群成员减少事件
/// </summary>
public delegate Task GroupDecreaseListener(NapcatBot bot, GroupDecreaseEvent e);
