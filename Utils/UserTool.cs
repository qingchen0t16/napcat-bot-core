namespace NapcatBot.Core.Utils;

public static class UserTool
{
    /// <summary>
    /// 获取Bkn(g_tk)
    /// </summary>
    /// <param name="sKey"></param>
    /// <returns></returns>
    public static string GetBknFromSKey(string sKey)
    {
        long hash = 5381;

        foreach (var c in sKey)
        {
            hash = hash + (hash << 5) + c;
        }

        return (hash & 0x7FFFFFFF).ToString();
    }
}