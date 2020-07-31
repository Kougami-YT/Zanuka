using Native.Sdk.Cqp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using WarframeStat_Class;
using HtmlAgilityPack;

namespace me.cqp.yt.zanuka.Code
{
    public static class Dic
    {
        public static Dictionary<string, string> dict = new Dictionary<string, string>(); //自动词库
        public static Dictionary<string, string> nightwave = new Dictionary<string, string>(); //电波任务词库
        public static Dictionary<string, string> invasion = new Dictionary<string, string>(); //入侵奖励词库
        public static List<Relic> relics = new List<Relic>();
        public static List<Dict> ParseDictJson(string url)
        {
            string json = Net.Get(url);
            if (string.IsNullOrEmpty(json)) json = "[]";
            try
            {
                return JsonConvert.DeserializeObject<List<Dict>>(json);
            }
            catch
            {
                return null;
            }
        } //网络Json文本转实体类
        public class Dict
        {
            public int id { get; set; }
            public string zh { get; set; }
            public string en { get; set; }
        }
        static string url_Dict = "https://raw.githubusercontent.com/Richasy/WFA_Lexicon/WFA5/WF_Dict.json";
        static string url_NightWave = "https://raw.githubusercontent.com/Richasy/WFA_Lexicon/WFA5/WF_NightWave.json";
        static string url_Invasion = "https://raw.githubusercontent.com/Richasy/WFA_Lexicon/WFA5/WF_Invasion.json";
        static string url_Relic = "https://warframe.huijiwiki.com/wiki/虚空遗物/奖励表/以遗物划分";
        public static DicResult GetOnline()
        {
            DicResult result = new DicResult();
            result.count_dict = GetOnlineDict(dict, url_Dict);
            result.count_nightwave = GetOnlineDict(nightwave, url_NightWave);
            result.count_invasion = GetOnlineDict(invasion, url_Invasion);
            result.count_relic = GetOnlineRelic(relics, url_Relic);
            SetLocal();
            return result;
        } 
        public static int GetOnlineDict(Dictionary<string, string> dict, string url)
        {
            List<Dict> dicts = ParseDictJson(url);
            if (dicts == null) return 0;
            dict.Clear();
            for (int i = 0; i < dicts.ToArray().Length; i++)
            {
                if (dict.ContainsKey(dicts[i].en)) continue;
                dict.Add(dicts[i].en, dicts[i].zh);
            }
            return dict.ToArray().Length;
        } //获取WFA词库
        public static int GetOnlineRelic(List<Relic> relics, string url)
        {
            HtmlDocument html = new HtmlDocument();
            HtmlNodeCollection tr = new HtmlNodeCollection(null);
            try
            {
                html.LoadHtml(Net.Get(url));
                tr = html.DocumentNode.SelectNodes("//tr[@class='filter-div--item']");
            }
            catch
            {
                return 0;
            }
            relics.Clear();
            foreach (HtmlNode i in tr)
            {
                Relic temp = new Relic();
                temp.name = i.SelectSingleNode("td/span").InnerText;
                temp.vault = i.GetAttributeValue("data-vault", "no") == "yes" ? true : false;
                temp.baro = i.GetAttributeValue("data-baro", "no") == "yes" ? true : false;
                temp.item_1 = i.SelectSingleNode("td[2]/p/a[1]").InnerText;
                temp.item_2 = i.SelectSingleNode("td[2]/p/a[2]").InnerText;
                temp.item_3 = i.SelectSingleNode("td[2]/p/a[3]").InnerText;
                temp.item_4 = i.SelectSingleNode("td[3]/p/a[1]").InnerText;
                temp.item_5 = i.SelectSingleNode("td[3]/p/a[2]").InnerText;
                temp.item_6 = i.SelectSingleNode("td[4]/p/a[1]").InnerText;
                relics.Add(temp);
            }
            return relics.ToArray().Length;
        } //获取WIKI遗物
        public static void GetLocal()
        {
            GetLocalDict(dict, "dict");
            GetLocalDict(nightwave, "nightwave");
            GetLocalDict(invasion, "invasion");
            GetLocalRelic(relics, "relics");
        }
        public static void GetLocalDict(Dictionary<string, string> dict, string name)
        {
            dict.Clear();
            int count = Convert.ToInt32(Config.Get("default.ini", "all", name, "0"));
            string[] en = Config.Read(Config.cjPath + "en_" + name).Split('\n');
            string[] zh = Config.Read(Config.cjPath + "zh_" + name).Split('\n');
            for (int i = 0; i < count; i++)
            {
                dict.Add(en[i], zh[i]);
            }
        } //读取本地词库
        public static void GetLocalRelic(List<Relic> relics, string name)
        {
            relics.Clear();
            int count = Convert.ToInt32(Config.Get(name + ".ini", "all", "count", "0"));
            for (int i = 0; i < count; i++)
            {
                Relic temp = new Relic();
                temp.name = Config.Get(name + ".ini", i.ToString(), "name");
                temp.vault = Config.Get(name + ".ini", i.ToString(), "vault", "0") == "1" ? true : false;
                temp.baro = Config.Get(name + ".ini", i.ToString(), "baro", "0") == "1" ? true : false;
                temp.item_1 = Config.Get(name + ".ini", i.ToString(), "item_1");
                temp.item_2 = Config.Get(name + ".ini", i.ToString(), "item_2");
                temp.item_3 = Config.Get(name + ".ini", i.ToString(), "item_3");
                temp.item_4 = Config.Get(name + ".ini", i.ToString(), "item_4");
                temp.item_5 = Config.Get(name + ".ini", i.ToString(), "item_5");
                temp.item_6 = Config.Get(name + ".ini", i.ToString(), "item_6");
                relics.Add(temp);
            }
        } //读取本地遗物
        public static void SetLocal()
        {
            SetLocalDict(dict, "dict");
            SetLocalDict(nightwave, "nightwave");
            SetLocalDict(invasion, "invasion");
            SetLocalRelic(relics, "relics");
        }
        public static void SetLocalDict(Dictionary<string, string> dict, string name)
        {
            int count = dict.ToArray().Length;
            StringBuilder en = new StringBuilder();
            StringBuilder zh = new StringBuilder();
            foreach (KeyValuePair<string, string> i in dict)
            {
                en.AppendFormat("{0}\n", i.Key);
                zh.AppendFormat("{0}\n", i.Value);
            }
            Config.Write(Config.cjPath + "en_" + name, Config.TrimE(en.ToString()));
            Config.Write(Config.cjPath + "zh_" + name, Config.TrimE(zh.ToString()));
            Config.Set("default.ini", "all", name, count.ToString());
        } //写入本地词库
        public static void SetLocalRelic(List<Relic> relics, string name)
        {
            int count = relics.ToArray().Length;
            Config.Set(name + ".ini", "all", "count", count.ToString());
            for (int i = 0; i < count; i++)
            {
                Config.Set(name + ".ini", i.ToString(), "name", relics[i].name);
                Config.Set(name + ".ini", i.ToString(), "vault", relics[i].vault ? "1" : "0");
                Config.Set(name + ".ini", i.ToString(), "baro", relics[i].baro ? "1" : "0");
                Config.Set(name + ".ini", i.ToString(), "item_1", relics[i].item_1);
                Config.Set(name + ".ini", i.ToString(), "item_2", relics[i].item_2);
                Config.Set(name + ".ini", i.ToString(), "item_3", relics[i].item_3);
                Config.Set(name + ".ini", i.ToString(), "item_4", relics[i].item_4);
                Config.Set(name + ".ini", i.ToString(), "item_5", relics[i].item_5);
                Config.Set(name + ".ini", i.ToString(), "item_6", relics[i].item_6);
            }
        } //写入本地遗物
        public readonly static Dictionary<string, string> custDic = new Dictionary<string, string>()
        {
            {"bp", "Blueprint"},
            {"总图", "Blueprint"},
            {"蓝图", "Blueprint"},
            {"图", "Blueprint"},
            {"机体", "Chassis"},
            {"系统", "Systems"},
            {"头", "Neuroptics"},
            {"枪机", "Receiver"},
            {"枪管", "Barrel"},
            {"枪托", "Stock"},
            {"刀刃", "Blade"},
            {"镖袋", "Pouch"},
            {"连接器", "Link"},
            {"握柄", "Hilt"},
            {"手套", "Gauntlet"},
            {"圆盘", "Disc"},
            {"下弓臂", "Lower Limb"},
            {"上弓臂", "Upper Limb"},
            {"弓臂", "limbs"},
            {"护手", "Guard"},
            {"靴子", "Boot"},
            {"外壳", "Carapace"},
        }; //自定义词库
        public static string ItemHandleWM(string item) //物品词库处理(WM版)
        {
            foreach (KeyValuePair<string, string> i in custDic)
            {
                item = item.Replace(i.Key, i.Value);
            }
            if (item.Length > 5 && item.ToLower().Substring(item.Length - 5, 5) == "prime") item += " Set";
            if (item.Length > 3 && item.ToLower().Substring(item.Length - 3, 3) == "mod") item += " (Veiled)";
            string head = Sub.Mid(item + " ", "", " ");
            string body = item.Replace(head, "");
            foreach (KeyValuePair<string, string> i in dict)
            {
                if (i.Value == head)
                {
                    head = i.Key;
                    break;
                }
            }
            string result = "";
            string[] temp = head.Split(',');
            if (temp.Length == 1)
            {
                result = head + body;
            }
            else
            {
                foreach (string i in temp)
                {
                    result += i + body + ",";
                }
                result = result.Substring(result.Length - 1, 1);
            }
            return result;
        }
        public static string ItemHandleRM(string item) //物品词库处理(RM版)
        {
            string head = Sub.Mid(item + " ", "", " ");
            string body = item.Replace(head, "");
            foreach (KeyValuePair<string, string> i in dict)
            {
                if (i.Value == head)
                {
                    head = i.Key;
                    break;
                }
            }
            string result = "";
            string[] temp = head.Split(',');
            if (temp.Length == 1)
            {
                result = head + body;
            }
            else
            {
                foreach (string i in temp)
                {
                    result += i + body + ",";
                }
                result = result.Substring(result.Length - 1, 1);
            }
            return result;
        }
        public static string EnToZh(string en, Dictionary<string, string> dic)
        {
            if (dic.ContainsKey(en)) return dic[en];
            return en;
        } //英译中

    }
    public class DicResult
    {
        public int count_dict { get; set; }
        public int count_nightwave { get; set; }
        public int count_invasion { get; set; }
        public int count_relic { get; set; }
    }
    public class Relic
    {
        public string name { get; set; }
        public bool vault { get; set; }
        public bool baro { get; set; }
        public string tier { get; set; }
        public string item_1 { get; set; }
        public string item_2 { get; set; }
        public string item_3 { get; set; }
        public string item_4 { get; set; }
        public string item_5 { get; set; }
        public string item_6 { get; set; }
    }
}

