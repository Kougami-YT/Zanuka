using me.cqp.yt.zanuka.Code.Tools;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.Enum;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace me.cqp.yt.zanuka.Code
{
    public class Event_GroupMessage : IGroupMessage
    {
        /// <summary>
        /// 收到群消息
        /// </summary>
        /// <param name="sender">事件来源</param>
        /// <param name="e">事件参数</param>
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            string msg = e.Message.Text;
            
            if (e.FromQQ == Config.qqAdmin && msg.Contains("Debug"))
            {
                e.FromGroup.SendGroupMessage(msg);
            } //测试用
            
            if (msg.IndexOf(Code.At(e.CQApi.GetLoginQQId())) > -1)
            {
                if (GroupMan.Is(e.FromGroup.Id))
                {
                    e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "插件运行中\n" + Menu());
                }
                else
                {
                    e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "插件未启动\n请物理管理员输入[*启动]来开启本插件");
                }
                if (e.FromQQ != Config.qqAdmin && Ran.Int(10) < 1) e.FromGroup.SendGroupMessage(Code.Pic("主动.jpg")); //恶搞
            } //自身被@
            else if (msg == "*启动")
            {
                if (e.FromQQ.Id == Config.qqAdmin)
                {
                    if (GroupMan.Is(e.FromGroup.Id))
                    {
                        e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "插件运行中\n@本机器人查看菜单");
                    }
                    else
                    {
                        GroupMan.Add(e.FromGroup.Id);
                        e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "插件启动成功！");
                    }
                }
                else
                {
                    e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "权限不足！");
                }
            } //插件启动指令
            else if (GroupMan.Is(e.FromGroup.Id))
            {
                if (msg == "*关闭")
                {
                    if (e.FromQQ.Id == Config.qqAdmin)
                    {
                        GroupMan.Remove(e.FromGroup.Id);
                        e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "插件关闭成功！");
                    }
                    else
                    {
                        e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "权限不足！");
                    }
                } //插件关闭指令
                else //插件内容 以下为插件运行时触发
                {
                    Sub.Sep(msg, out string order, out string param); //分割指令名与参数
                    foreach (KeyValuePair<string, Function.FuncG> i in instruct)
                    {
                        if (i.Key == order.ToLower())
                        {
                            if (MemberMan.GetIdRank(e) == "*nothing*")
                            {
                                e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "\n群名片格式错误！禁止使用本插件！\n正确格式：XXX【X段】\nPS：请检查多余符号或空格");
                                return;
                            } //群名片不规范
                            FuncResult result = i.Value(e, param);
                            switch (result.type)
                            {
                                case 0:
                                    e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "\n" + result.text);
                                    break;
                                case 1:
                                    e.FromGroup.SendGroupMessage(Code.At(e.FromQQ.Id) + "权限不足！");
                                    break;
                            }
                            if (e.FromQQ != Config.qqAdmin && Ran.Int(10) < 1) e.FromGroup.SendGroupMessage(Code.Pic("被动.jpg")); //恶搞
                            return;
                        }
                    }

                    #region --复读--
                    if (msg == Reread.last)
                    {
                        if (msg != Reread.lastR)
                        {
                            e.FromGroup.SendGroupMessage(msg);
                            Reread.lastR = msg;
                        }
                    }
                    else
                    {
                        Reread.lastR = "";
                    }
                    Reread.last = msg;
                    #endregion

                    #region --表情包--
                    if (Ran.Int(100) < 1)
                    {
                        int count = Emoticons.Get();
                        if (count > 0) e.FromGroup.SendGroupMessage(Code.Pic(Emoticons.files[Ran.Int(count)]));
                    }
#endregion
                }
            }



            // 设置该属性, 表示阻塞本条消息, 该属性会在方法结束后传递给酷Q
            e.Handler = true;
        }




        
        static string Menu()
        {
            string result = "ZANUKA船新版本2.0内测版功能如下：\n";
            foreach (KeyValuePair<string, Function.FuncG> i in instruct)
            {
                result += "|" + i.Key;
            }
            result += "|\n";
            result += "......待添加";
            return result;
        } //菜单文本
        static readonly Dictionary<string, Function.FuncG> instruct = new Dictionary<string, Function.FuncG>() //指令字典 无参数
        {
            { "平原时间" , new Function.FuncG(new Function().CetusCycle)},
            { "地球时间" , new Function.FuncG(new Function().EarthCycle)},
            { "山谷时间" , new Function.FuncG(new Function().VallisCycle)},
            { "仲裁" , new Function.FuncG(new Function().Arbitration)},
            { "突击" , new Function.FuncG(new Function().Sortie)},
            { "入侵" , new Function.FuncG(new Function().Invasions)},
            { "裂隙" , new Function.FuncG(new Function().Fissures)},
            { "奸商" , new Function.FuncG(new Function().VoidTrader)},
            { "电波" , new Function.FuncG(new Function().Nightwave)},

            { "活动" , new Function.FuncG(new Function().Events)},
            { "警报" , new Function.FuncG(new Function().Alerts)},
            { "地球赏金" , new Function.FuncG(new Function().Ostrons)},
            { "金星赏金" , new Function.FuncG(new Function().Solaris)},
            { "特价" , new Function.FuncG(new Function().DailyDeals)},
            { "小小黑" , new Function.FuncG(new Function().PersistentEnemies)},
            { "舰队" , new Function.FuncG(new Function().ConstructionProgress)},
            { "wm" , new Function.FuncG(new Function().WarframeMarket)},
            { "rm" , new Function.FuncG(new Function().RivenMarket)},
            { "wiki" , new Function.FuncG(new Function().Wiki)},

            { "更新数据" , new Function.FuncG(new Function().UpdateData)},
            { "抽奖" , new Function.FuncG(new Function().Lottery)},
            { "开启抽奖" , new Function.FuncG(new Function().LotteryStart)},
            { "参加抽奖" , new Function.FuncG(new Function().LotteryJoin)},
            { "奖品" , new Function.FuncG(new Function().LotteryPrize)},
            { "提供奖品" , new Function.FuncG(new Function().LotteryAdd)},
            { "开奖结果" , new Function.FuncG(new Function().LotteryResult)},
            { "开奖" , new Function.FuncG(new Function().LotteryOpen)},

            { "添加监控" , new Function.FuncG(new Function().MonitorAdd)},
            { "删除监控" , new Function.FuncG(new Function().MonitorRemove)},
        };
    }
    public class FuncResult
    {
        public int type; //0正常返回文本 1权限不足
        public string text;
        public FuncResult(int type = 0, string text = "*nothing*")
        {
            this.type = type;
            this.text = text;
        }
    }
}
