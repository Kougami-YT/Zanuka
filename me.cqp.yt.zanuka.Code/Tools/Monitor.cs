using Native.Sdk.Cqp.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WarframeStat_Class;

namespace me.cqp.yt.zanuka.Code.Tools
{
    public static class Monitor
    {
        public static Timer timer = new Timer();
        public static CQAppEnableEventArgs CQe;
        public static List<string> keywords = new List<string>();
        public static void Start(CQAppEnableEventArgs e)
        {
            string[] temp = Config.Get("monitor.ini", "all", "keywords", "").Split(',');
            foreach (string i in temp)
            {
                if (i != "") keywords.Add(i);
            }
            CQe = e;
            timer.Enabled = true;
            timer.Interval = 60000;
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(Event_Timer);
        }
        private static void Event_Timer(object source, ElapsedEventArgs e)
        {
            List<Resp_Invasions> Info = API.JsonToClass<List<Resp_Invasions>>(API.url_Invasions);
            if (Info == null || Info.ToArray().Length == 0)
            {
                return;
            }
            else
            {
                foreach (Resp_Invasions i in Info)
                {
                    if (IsRecordExist(i)) continue;
                    foreach (string j in keywords)
                    {
                        if (i.attackerReward.countedItems.Length != 0 && Dic.EnToZh(i.attackerReward.countedItems[0].key, Dic.invasion).ToLower().Contains(j.ToLower()) ||
                            i.defenderReward.countedItems.Length != 0 && Dic.EnToZh(i.defenderReward.countedItems[0].key, Dic.invasion).ToLower().Contains(j.ToLower()))
                        {
                            string msg = "监控到关键词 [" + j + "]\n——————————";
                            msg += "\n—节点：" + i.node.Replace(Sub.Mid(i.node, "(", ")"), Dic.EnToZh(Sub.Mid(i.node, "(", ")"), Dic.dict));
                            msg += "\n—进攻：" + Convert.ToInt32(i.completion).ToString() + "% ";
                            msg += i.attackerReward.countedItems.Length == 0 ? "无" : Dic.EnToZh(i.attackerReward.countedItems[0].key, Dic.invasion) + "×" + i.attackerReward.countedItems[0].count.ToString();
                            msg += "\n—防守：" + (100 - Convert.ToInt32(i.completion)).ToString() + "% ";
                            msg += i.defenderReward.countedItems.Length == 0 ? "无" : Dic.EnToZh(i.defenderReward.countedItems[0].key, Dic.invasion) + "×" + i.defenderReward.countedItems[0].count.ToString();
                            msg += "\n——————————";
                            foreach (long k in GroupMan.GroupIDs)
                            {
                                CQe.CQApi.SendGroupMessage(k, msg);
                            }
                            Config.Set("monitor.ini", "all", "count", (Convert.ToInt32(Config.Get("monitor.ini", "all", "count", "0")) + 1).ToString());
                            Config.Set("monitor.ini", "records", (Convert.ToInt32(Config.Get("monitor.ini", "all", "count", "0")) - 1).ToString(), i.id);
                            break;
                        }
                    }
                }
            }
        }
        private static bool IsRecordExist(Resp_Invasions info)
        {
            int count = Convert.ToInt32(Config.Get("monitor.ini", "all", "count", "0"));
            for (int i = 0; i < count; i++)
            {
                string record = Config.Get("monitor.ini", "records", i.ToString(), "");
                if (record == info.id) return true;
            }
            return false;
        } //查询记录是否存在
    }
}
