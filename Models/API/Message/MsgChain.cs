using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Serilog;

namespace NapcatBot.Core.Models.API.Message;

/// <summary>
/// 消息链
/// </summary>
public class MsgChain : List<IMsgEntity>
{
    public MsgChain()
    {
    }

    public MsgChain(string text)
    {
        AddText(text);
    }

    /// <summary>
    /// 编制MsgChain
    /// 收到消息用的, 其他地方用不着
    /// </summary>
    /// <param name="jAry"></param>
    /// <returns></returns>
    public static MsgChain Parse(JArray jAry)
    {
        var chain = new MsgChain();
        foreach (var item in jAry)
        {
            var type = item["type"]?.Value<string>();
            if (type is null)
                continue;
            switch (type)
            {
                case "at":
                    chain.Add(item.ToObject<MsgEntity<MsgAtEntity>>() ?? throw new("转换类型失败"));
                    break;
                case "text":
                    var textEntity = item.ToObject<MsgEntity<MsgTextEntity>>() ?? throw new("转换类型失败");
                    if(string.IsNullOrEmpty(textEntity.Data.Text))
                        break;
                    chain.Add(textEntity);
                    break;
                case "reply":
                    chain.Add(item.ToObject<MsgEntity<MsgReplyEntity>>() ?? throw new("转换类型失败"));
                    break;
            }
        }

        return chain;
    }

    #region 各类方法

    /// <summary>
    /// 尝试获取回复的MsgID
    /// </summary>
    /// <param name="msgId"></param>
    /// <returns></returns>
    public bool TryGetReplyMsgID(out string? msgId)
    {
        if (this.FirstOrDefault() is MsgEntity<MsgReplyEntity> replay)
        {
            msgId = replay.Data.MsgId;
            return true;
        }

        msgId = null;
        return false;
    }

    /// <summary>
    /// 获取首个AT的QQ
    /// </summary>
    /// <param name="qq"></param>
    /// <returns></returns>
    public bool TryGetAtQQ(out ulong? qq)
    {
        try
        {
            if (Count == 0 || this[0] is not MsgEntity<MsgAtEntity> at)
            {
                qq = null;
                return false;
            }

            qq = Convert.ToUInt64(at.Data.QQ);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            qq = null;
            return false;
        }
    }
    /// <summary>
    /// 获取AT的QQ
    /// </summary>
    /// <param name="index"></param>
    /// <param name="qq"></param>
    /// <returns></returns>
    public bool TryGetAtQQ(int index,out ulong qq)
    {
        try
        {
            if (this.Count <= index || 
                this[index] is not MsgEntity<MsgAtEntity> at)
            {
                qq = 0;
                return false;
            }

            qq = Convert.ToUInt64(at.Data.QQ);
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            qq = 0;
            return false;
        }
    }

    /// <summary>
    /// 快捷获取简单文本
    /// </summary>
    /// <returns></returns>
    public string GetText()
    {
        return string.Join("",this.FindAll(r => r is MsgEntity<MsgTextEntity>).Select(r => (r as MsgEntity<MsgTextEntity>)?.Data.Text)).Trim();
    }

    #endregion

    /// <summary>
    /// 添加文本
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public MsgChain AddText(string text)
    {
        Add(MsgEntity<MsgTextEntity>.Create(new()
        {
            Text = text
        }));
        return this;
    }
    
    /// <summary>
    /// 添加文本
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public MsgChain AddText(StringBuilder text)
    {
        Add(MsgEntity<MsgTextEntity>.Create(new()
        {
            Text = text.ToString()
        }));
        return this;
    }

    /// <summary>
    /// AT某人
    /// </summary>
    /// <param name="qq"></param>
    /// <param name="pushFirst"></param>
    /// <returns></returns>
    public MsgChain At(ulong qq, bool pushFirst = false)
    {
        var entity = MsgEntity<MsgAtEntity>.Create(new ()
        {
            QQ = qq.ToString()
        });
        
        if (pushFirst)
            Insert(0, entity);
        else
            Add(entity);
        return this;
    }

    /// <summary>
    /// AT全体成员
    /// </summary>
    /// <returns></returns>
    public MsgChain AtAll()
    {
        Add(MsgEntity<MsgAtEntity>.Create(MsgAtEntity.AtAll()));
        return this;
    }

    /// <summary>
    /// 回复某个消息
    /// </summary>
    /// <param name="messageId">消息ID</param>
    /// <param name="pushFirst">是否添加至头(@就在首部)</param>
    /// <returns></returns>
    public MsgChain Reply(ulong messageId, bool pushFirst = false)
    {
        return Reply(messageId.ToString(), pushFirst);
    }

    /// <summary>
    /// 回复某个消息
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="pushFirst"></param>
    /// <returns></returns>
    public MsgChain Reply(string messageId, bool pushFirst = false)
    {
        var entity = MsgEntity<MsgReplyEntity>.Create(new()
        {
            MsgId = messageId
        });

        if (pushFirst)
            Insert(0, entity);
        else
            Add(entity);
        return this;
    }

    /// <summary>
    /// 插入图片
    /// </summary>
    /// <param name="imgUrl"></param>
    /// <param name="pushFirst"></param>
    /// <returns></returns>
    public MsgChain Image(string imgUrl, bool pushFirst = false)
    {
        var entity = MsgEntity<MsgImageEntity>.Create(new()
        {
            FilePath = imgUrl
        });

        if (pushFirst)
            Insert(0, entity);
        else
            Add(entity);
        return this;
    }
}