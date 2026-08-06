using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NapcatBot.Core.Models.Core;

/// <summary>
/// WS的等待任务
/// 发数据 -> 等待 -> 拿到数据且echo匹配 -> 执行下一步
/// </summary>
public class WebsocketTask
{
    /// <summary>
    /// 任务ID
    /// </summary>
    public string TaskID { get; set; } = string.Empty;
    /// <summary>
    /// 发送数据
    /// </summary>
    public object SendData { get; set; } = null!;
    /// <summary>
    /// Task
    /// </summary>
    public TaskCompletionSource<JObject> Task { get; set; } = new();
    /// <summary>
    /// 启动时间
    /// </summary>
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }
}