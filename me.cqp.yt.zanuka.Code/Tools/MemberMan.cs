using Native.Sdk.Cqp;
using Native.Sdk.Cqp.Enum;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.yt.zanuka.Code
{
    public static class MemberMan
    {
        public static string GetCard(CQGroupMessageEventArgs e, long qq = 0)
        {
            if (qq == 0) qq = e.FromQQ.Id;
            return e.FromGroup.GetGroupMemberInfo(new QQ(e.CQApi, qq), true).Card;
        } //获取群名片
        public static string GetIdRank(CQGroupMessageEventArgs e, long qq = 0)
        {
            if (qq == 0) qq = e.FromQQ.Id;
            string card = GetCard(e, qq);
            string id = "", rank = "";
            foreach(char i in card)
            {
                if (i >= 'A' && i <= 'Z' || 
                    i >= 'a' && i <= 'z' || 
                    i >= '0' && i <= '9' || 
                    i == '_' || i == '-' || i == '.')
                {
                    id += i;
                }
                else
                {
                    rank = Sub.Mid(card, id + "【", "段】");
                    if (rank != "*nothing*" && rank != "") return id + "【" + rank + "段】";
                    return "*nothing*";
                }
            }
            return "*nothing*";
        } //判断群名片是否符合要求
        public static bool IsCreator(CQGroupMessageEventArgs e)
        {
            return e.FromGroup.GetGroupMemberInfo(e.FromQQ, true).MemberType == QQGroupMemberType.Creator;
        } //判断是否为群主
        public static bool IsAdmin(CQGroupMessageEventArgs e)
        {
            return e.FromGroup.GetGroupMemberInfo(e.FromQQ, true).MemberType == QQGroupMemberType.Manage;
        } //判断是否为管理员
    }
}
