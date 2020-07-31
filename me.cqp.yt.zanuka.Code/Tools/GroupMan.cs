using System;
using System.Collections.Generic;

namespace me.cqp.yt.zanuka.Code
{
    public static class GroupMan
    {
        public static List<long> GroupIDs = new List<long>();
        public static void Get() //获取管理群信息
        {
            string[] temp = Config.Get("default.ini", "all", "group").Split(',');
            GroupIDs.Clear();
            if (temp.Length == 1 && temp[0] == "")
            {
                return;
            }
            foreach (string i in temp)
            {
                GroupIDs.Add(long.Parse(i));
            }
        }
        public static void Add(long GroupID) //添加群
        {
            if (GroupIDs.ToArray().Length == 0)
            {
                Config.Set("default.ini", "all", "group", GroupID.ToString());
            }
            else
            {
                Config.Set("default.ini", "all", "group", Config.Get("default.ini", "all", "group") + "," + GroupID.ToString());
            }
            GroupIDs.Add(GroupID);
        }
        public static void Remove(long GroupID) //删除群
        {
            if (GroupIDs[0] == GroupID)
            {
                if (GroupIDs.ToArray().Length == 1)
                {
                    Config.Set("default.ini", "all", "group", "");
                }
                else
                {
                    Config.Set("default.ini", "all", "group", Config.Get("default.ini", "all", "group").Replace(GroupID + ",", ""));
                }
            }
            else
            {
                Config.Set("default.ini", "all", "group", Config.Get("default.ini", "all", "group").Replace("," + GroupID, ""));
            }
            GroupIDs.Remove(GroupID);
        }
        public static bool Is(long GroupID) //判断是否为管理群
        {
            foreach (long i in GroupIDs)
            {
                if (i == GroupID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
