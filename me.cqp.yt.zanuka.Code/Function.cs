using json.nymph_wm;
using json.nymph_rm;
using json.WM;
using System;
using System.Collections.Generic;
using System.Net;
using Native.Sdk.Cqp;
using System.Data;
using Native.Sdk.Cqp.EventArgs;
using System.Linq;
using System.Text.RegularExpressions;
using WarframeStat_Class;
using me.cqp.yt.zanuka.Code.Tools;

namespace me.cqp.yt.zanuka.Code
{
    public class Function
    {
        public delegate FuncResult FuncG(CQGroupMessageEventArgs e, string param);
        public delegate FuncResult FuncP(CQPrivateMessageEventArgs e, string param);

        public FuncResult CetusCycle(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            Resp_Cetus Info = API.JsonToClass<Resp_Cetus>(API.url_Cetus);
            if (Info == null)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "Zanuka为恁报时：\n平原状态：";
                result.text += Info.isDay ? "白天" : "黑夜";
                result.text += "\n时间剩余：" + TimeHandle(Info.expiry.AddHours(8) - DateTime.Now);
            }
            return result;
        } //平原时间
        public FuncResult EarthCycle(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            Resp_Earth Info = API.JsonToClass<Resp_Earth>(API.url_Earth);
            if (Info == null)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "Zanuka为恁报时：\n地球状态：";
                result.text += Info.isDay ? "白天" : "黑夜";
                result.text += "\n时间剩余：" + TimeHandle(Info.expiry.AddHours(8) - DateTime.Now);
            }
            return result;
        } //地球时间
        public FuncResult VallisCycle(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            Resp_Vallis Info = API.JsonToClass<Resp_Vallis>(API.url_Vallis);
            if (Info == null)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "Zanuka为恁报时：\n山谷状态：";
                result.text += Info.state == "cold" ? "寒冷" : "温暖";
                result.text += "\n时间剩余：" + TimeHandle(Info.expiry.AddHours(8) - DateTime.Now);
            }
            return result;
        } //山谷
        public FuncResult Arbitration(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            Resp_Arbitration Info = API.JsonToClass<Resp_Arbitration>(API.url_Arbitration);
            if (Info == null)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "仲裁信息如下：\n节点：" + Info.node.Replace(Sub.Mid(Info.node, "(", ")"), Dic.EnToZh(Sub.Mid(Info.node, "(", ")"), Dic.dict));
                result.text += "\n任务：" + Dic.EnToZh(Info.type, Dic.dict) + "\n派系：" + Info.enemy;
                result.text += "\n时间剩余：" + TimeHandle(Info.expiry.AddHours(8) - DateTime.Now);
            }
            return result;
        } //仲裁
        public FuncResult Sortie(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            Resp_Sortie Info = API.JsonToClass<Resp_Sortie>(API.url_Sortie);
            if (Info == null)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "今日突击如下：\nBOSS：" + Info.boss + "(" + Info.faction + ")\n——————————";
                for (int i = 0; i < Info.variants.Length; i++)
                {
                    result.text += "\n—节点：" + Info.variants[i].node.Replace(Sub.Mid(Info.variants[i].node, "(", ")"), Dic.EnToZh(Sub.Mid(Info.variants[i].node, "(", ")"), Dic.dict));
                    result.text += "\n—任务：" + Dic.EnToZh(Info.variants[i].missionType, Dic.dict);
                    if (Info.variants[i].modifier.Contains(":"))
                    {
                        string[] temp = Info.variants[i].modifier.Split(':');
                        Info.variants[i].modifier = Dic.EnToZh(temp[0].Trim(), Dic.dict) + ":" + Dic.EnToZh(temp[1].Trim(), Dic.dict);
                    }
                    else
                    {
                        Info.variants[i].modifier = Dic.EnToZh(Info.variants[i].modifier, Dic.dict);
                    }
                    result.text += "(" + Info.variants[i].modifier + ")\n——————————";
                }
                result.text += "\n时间剩余：" + TimeHandle(Info.expiry.AddHours(8) - DateTime.Now);
            }
            return result;
        } //突击
        public FuncResult Invasions(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            List<Resp_Invasions> Info = API.JsonToClass<List<Resp_Invasions>>(API.url_Invasions);
            if (Info == null || Info.ToArray().Length == 0)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "入侵信息如下：\n——————————";
                foreach (Resp_Invasions i in Info)
                {
                    result.text += "\n—节点：" + i.node.Replace(Sub.Mid(i.node, "(", ")"), Dic.EnToZh(Sub.Mid(i.node, "(", ")"), Dic.dict));
                    result.text += "\n—进攻：" + Convert.ToInt32(i.completion).ToString() + "% ";
                    result.text += i.attackerReward.countedItems.Length == 0 ? "无" : Dic.EnToZh(i.attackerReward.countedItems[0].key, Dic.invasion) + "×" + i.attackerReward.countedItems[0].count.ToString();
                    result.text += "\n—防守：" + (100 - Convert.ToInt32(i.completion)).ToString() + "% ";
                    result.text += i.defenderReward.countedItems.Length == 0 ? "无" : Dic.EnToZh(i.defenderReward.countedItems[0].key, Dic.invasion) + "×" + i.defenderReward.countedItems[0].count.ToString();
                    result.text += "\n——————————";
                }
            }
            return result;
        } //入侵
        public FuncResult Fissures(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            List<Resp_Fissures> Info = API.JsonToClass<List<Resp_Fissures>>(API.url_Fissures);
            if (Info == null || Info.ToArray().Length == 0)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "裂隙信息如下：\n——————————";
                for (int tier = 0; tier < 5; tier++)
                {
                    foreach(Resp_Fissures i in Info)
                    {
                        if (i.tierNum != tier + 1) continue;
                        result.text += "\n—[" + Dic.EnToZh(i.tier, Dic.dict) + "]" + i.node.Replace(Sub.Mid(i.node, "(", ")"), Dic.EnToZh(Sub.Mid(i.node, "(", ")"), Dic.dict));
                        result.text += "\n—模式：" + Dic.EnToZh(i.missionType, Dic.dict) + "(" + i.enemy + ")";
                        result.text += "\n—时间剩余：" + TimeHandle(i.expiry.AddHours(8) - DateTime.Now);
                        result.text += "\n——————————";
                    }
                }
            }
            return result;
        } //裂隙
        public FuncResult DailyDeals(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            List<Resp_DailyDeals> Info = API.JsonToClass<List<Resp_DailyDeals>>(API.url_DailyDeals);
            if (Info == null || Info.ToArray().Length == 0)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "特价商品如下：";
                result.text += "\n商品：" + Dic.EnToZh(Info[0].item, Dic.dict) + " [-" + Info[0].discount + "%]";
                result.text += "\n售价：" + Info[0].salePrice + "(" + Info[0].originalPrice + ")";
                result.text += "\n库存：" + (Info[0].total - Info[0].sold) + "/" + Info[0].total;
                result.text += "\n时间剩余：" + TimeHandle(Info[0].expiry.AddHours(8) - DateTime.Now);
            }
            return result;
        } //特价
        public FuncResult VoidTrader(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            Resp_VoidTrader Info = API.JsonToClass<Resp_VoidTrader>(API.url_VoidTrader);
            if (Info == null)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else if (!Info.active)
            {
                result.text = "距离 " + Info.character + " 到达 " + Dic.EnToZh(Sub.Mid(Info.location, "(", ")"), Dic.dict) + " 中继站还有 ";
                result.text += TimeHandle(Info.activation.AddHours(8) - DateTime.Now);
            }
            else
            {
                result.text = Info.character + " 已到达 " + Dic.EnToZh(Sub.Mid(Info.location, "(", ")"), Dic.dict) + " 中继站\n——————————";
                for (int i = 0; i < Info.inventory.Length; i++)
                {
                    result.text += "\n—" + Dic.EnToZh(Info.inventory[i].item, Dic.dict) + " [" + Info.inventory[i].ducats + " 杜卡德] [" + Info.inventory[i].credits + " 现金]";
                }
                result.text += "\n——————————";
                result.text += "\n时间剩余：" + TimeHandle(Info.expiry.AddHours(8) - DateTime.Now);
            }
            return result;
        } //奸商
        public FuncResult Nightwave(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            Resp_Nightwave Info = API.JsonToClass<Resp_Nightwave>(API.url_Nightwave);
            if (Info == null)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = "电波任务如下：\n——————————";
                for (int i = 0; i < Info.activeChallenges.Length; i++)
                {
                    result.text += "\n—[" + Info.activeChallenges[i].reputation + "] " + Dic.EnToZh(Info.activeChallenges[i].title, Dic.nightwave);
                    result.text += "\n—任务：" + Dic.EnToZh(Info.activeChallenges[i].desc, Dic.nightwave);
                    result.text += "\n—时间剩余：" + TimeHandle(Info.activeChallenges[i].expiry.AddHours(8) - DateTime.Now);
                    result.text += "\n——————————";
                }
                result.text += "\n本季电波剩余时间：" + TimeHandle(Info.expiry.AddHours(8) - DateTime.Now);
            }
            return result;
        } //电波

        public FuncResult Events(CQGroupMessageEventArgs e, string param) //活动
        {
            FuncResult result = new FuncResult();
            result.text = "游戏内活动如下：\n" + Net.Get("http://nymph.rbq.life:3000/wf/robot/events");
            return result;
        }
        public FuncResult Alerts(CQGroupMessageEventArgs e, string param) //警报
        {
            FuncResult result = new FuncResult();
            result.text = "警报如下：\n" + Net.Get("http://nymph.rbq.life:3000/wf/robot/alerts");
            return result;
        }
        public FuncResult Ostrons(CQGroupMessageEventArgs e, string param) //地球赏金
        {
            FuncResult result = new FuncResult();
            result.text = "地球赏金如下：\n" + Net.Get("http://nymph.rbq.life:3000/wf/robot/Ostrons");
            return result;
        }
        public FuncResult Solaris(CQGroupMessageEventArgs e, string param) //金星赏金
        {
            FuncResult result = new FuncResult();
            result.text = "金星赏金如下：\n" + Net.Get("http://nymph.rbq.life:3000/wf/robot/Solaris");
            return result;
        }
        public FuncResult PersistentEnemies(CQGroupMessageEventArgs e, string param) //小小黑
        {
            FuncResult result = new FuncResult();
            result.text = "小小黑信息如下：\n" + Net.Get("http://nymph.rbq.life:3000/wf/robot/persistentEnemies");
            return result;
        }
        public FuncResult ConstructionProgress(CQGroupMessageEventArgs e, string param) //舰队
        {
            FuncResult result = new FuncResult();
            result.text = "舰队信息如下：\n" + Net.Get("http://nymph.rbq.life:3000/wf/robot/constructionProgress");
            return result;
        }




        public FuncResult UpdateData(CQGroupMessageEventArgs e, string param) //更新数据
        {
            FuncResult result = new FuncResult();
            if (e.FromQQ == Config.qqAdmin)
            {
                e.FromGroup.SendGroupMessage("数据更新中......");
                DicResult count = Dic.GetOnline();
                result.text = "数据更新成功！";
                result.text += "\n常规词库：" + count.count_dict;
                result.text += "\n电波词库：" + count.count_nightwave;
                result.text += "\n入侵词库：" + count.count_invasion;
                result.text += "\n遗物数据：" + count.count_relic;
            }
            else
            {
                result.type = 1;
            }
            return result;
        }
        public FuncResult Lottery(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (LotteryMan.on)
            {
                result.text = "抽奖活动进行中\n[提供奖品 物品(,数量)]添加奖品\n[参加抽奖]参加本次抽奖\n[奖品]查看奖池\n[开奖]仅限发起人";
            }
            else
            {
                result.text = "目前没有抽奖活动\n[开启抽奖]仅限管理员\n[开奖结果]查看抽奖记录";
            }
            return result;
        } //抽奖
        public FuncResult LotteryStart(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (LotteryMan.on)
            {
                result.text = "抽奖活动进行中";
            }
            else if(MemberMan.IsAdmin(e) || MemberMan.IsCreator(e))
            {
                LotteryMan.on = true;
                Config.Set("Lottery.ini", "all", "on", "1");
                LotteryMan.count += 1;
                Config.Set("Lottery.ini", "all", "count", LotteryMan.count.ToString());
                Config.Set("Lottery.ini", LotteryMan.count.ToString(), "admin", e.FromQQ.Id.ToString());
                result.text = "抽奖活动已开启\n[提供奖品 物品(,数量)]添加奖品\n[参加抽奖]参加本次抽奖\n[奖品]查看奖池\n[开奖]仅限发起人";
            }
            else
            {
                result.type = 1;
            }
            return result;
        } //开启抽奖
        public FuncResult LotteryJoin(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (LotteryMan.on)
            {
                if (Config.Get("Lottery.ini", LotteryMan.count.ToString(), "member").Contains(e.FromQQ.Id.ToString()))
                {
                    result.text = "你已在列表中";
                }
                else
                {
                    if (Config.Get("Lottery.ini", LotteryMan.count.ToString(), "member") == "")
                    {
                        Config.Set("Lottery.ini", LotteryMan.count.ToString(), "member", e.FromQQ.Id.ToString());
                    }
                    else
                    {
                        Config.Set("Lottery.ini", LotteryMan.count.ToString(), "member", Config.Get("Lottery.ini", LotteryMan.count.ToString(), "member") + "," + e.FromQQ.Id.ToString());
                    }
                    result.text = "参加活动成功,请等待开奖";
                }
            }
            else
            {
                result.text = "目前没有抽奖活动";
            }
            return result;
        } //参加抽奖
        public FuncResult LotteryPrize(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (LotteryMan.on)
            {
                int count = Convert.ToInt32(Config.Get("Lottery.ini", LotteryMan.count.ToString(), "count", "0"));
                if (count == 0)
                {
                    result.text = "暂无奖品";
                }
                else
                {
                    result.text = "——————————\n";
                    for (int i = 0; i < count; i++)
                    {
                        string prize = Config.Get("Lottery.ini", LotteryMan.count.ToString(), "prize" + (i + 1).ToString());
                        string pcount = Config.Get("Lottery.ini", LotteryMan.count.ToString(), "pcount" + (i + 1).ToString());
                        string pmember = Config.Get("Lottery.ini", LotteryMan.count.ToString(), "pmember" + (i + 1).ToString());
                        result.text += "—";
                        result.text += prize + "×" + pcount + "(提供者:" + MemberMan.GetIdRank(e, Convert.ToInt64(pmember)) + ")\n";
                    }
                    result.text += "——————————\n";
                    result.text += "参加者:|";
                    string[] member = Config.Get("Lottery.ini", LotteryMan.count.ToString(), "member").Split(',');
                    if (member[0] == "")
                    {
                        result.text += "暂无";
                    }
                    else
                    {
                        foreach (string i in member) result.text += MemberMan.GetIdRank(e, Convert.ToInt64(i)) + "|";
                    }
                }
            }
            else
            {
                result.text = "目前没有抽奖活动";
            }
            return result;
        } //奖品
        public FuncResult LotteryResult(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            int Tcount = LotteryMan.count;
            if (LotteryMan.on) Tcount -= 1;
            if (new Regex("^[0-9]+$").IsMatch(param) && Convert.ToInt32(param) <= Tcount)
            {
                int count = Convert.ToInt32(Config.Get("Lottery.ini", param, "count", "0"));
                result.text = "——————————\n";
                for (int i = 0; i < count; i++)
                {
                    string prize = Config.Get("Lottery.ini", param, "prize" + (i + 1).ToString());
                    string pcount = Config.Get("Lottery.ini", param, "pcount" + (i + 1).ToString());
                    string pmember = Config.Get("Lottery.ini", param, "pmember" + (i + 1).ToString());
                    string pwinners = Config.Get("Lottery.ini", param, "pwinner" + (i + 1).ToString());
                    if (pwinners == "")
                    {
                        pwinners = "无";
                    }
                    else
                    {
                        string[] pwinner = pwinners.Split(',');
                        pwinners = "";
                        foreach (string j in pwinner)
                        {
                            pwinners += MemberMan.GetIdRank(e, Convert.ToInt64(j)) + ",";
                        }
                        pwinners = pwinners.Substring(0, pwinners.Length - 1);
                    }
                    result.text += "—";
                    result.text += prize + "×" + pcount + "(提供者:" + MemberMan.GetIdRank(e, Convert.ToInt64(pmember)) + ")(获奖者:" + pwinners  + ")\n";
                }
                result.text += "——————————\n";
            }
            else if (Tcount == 0)
            {
                result.text = "暂无记录";
            }
            else
            {
                result.text = "请选择记录序号:\n——————————\n";
                for (int i = 0; i < Tcount; i++)
                {
                    string member = Config.Get("Lottery.ini", (i + 1).ToString(), "admin");
                    string count = Config.Get("Lottery.ini", (i + 1).ToString(), "count");
                    result.text += "—【" + (i + 1).ToString() + "】—发起人:" + MemberMan.GetIdRank(e, Convert.ToInt64(member)) + "—奖品数:" + count + "\n";
                }
                result.text += "——————————";
                
            }
            return result;
        } //开奖结果
        public FuncResult LotteryOpen(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (LotteryMan.on)
            {
                if (e.FromQQ.Id.ToString() == Config.Get("Lottery.ini", LotteryMan.count.ToString(), "admin") || e.FromQQ == Config.qqAdmin)
                {
                    result.text = "恭喜以下成员获奖:\n——————————\n";
                    int count = Convert.ToInt32(Config.Get("Lottery.ini", LotteryMan.count.ToString(), "count"));
                    List<int> Tprize = new List<int>();
                    for (int i = 0; i < count; i++)
                    {
                        int pcount = Convert.ToInt32(Config.Get("Lottery.ini", LotteryMan.count.ToString(), "pcount" + (i + 1).ToString()));
                        for (int j = 0; j < pcount; j++) Tprize.Add(i);
                    }
                    List<long> Tmember = new List<long>();
                    string[] member = Config.Get("Lottery.ini", LotteryMan.count.ToString(), "member").Split(',');
                    foreach (string i in member) Tmember.Add(Convert.ToInt64(i));
                    while (Tprize.ToArray().Length > 0 && Tmember.ToArray().Length > 0)
                    {
                        int prize = Ran.Int(Tprize.ToArray().Length);
                        int pwinner = Ran.Int(Tmember.ToArray().Length);
                        result.text += "-" + Code.At(Tmember[pwinner]) + ":";
                        result.text += Config.Get("Lottery.ini", LotteryMan.count.ToString(), "prize" + (Tprize[prize] + 1).ToString());
                        result.text += "(提供者:" + MemberMan.GetIdRank(e, Convert.ToInt64(Config.Get("Lottery.ini", LotteryMan.count.ToString(), "pmember" + (Tprize[prize] + 1).ToString()))) + ")\n";
                        if (Config.Get("Lottery.ini", LotteryMan.count.ToString(), "pwinner" + (Tprize[prize] + 1).ToString(), "") == "")
                        {
                            Config.Set("Lottery.ini", LotteryMan.count.ToString(), "pwinner" + (Tprize[prize] + 1).ToString(), Tmember[pwinner].ToString());
                        }
                        else
                        {
                            Config.Set("Lottery.ini", LotteryMan.count.ToString(), "pwinner" + (Tprize[prize] + 1).ToString(), Config.Get("Lottery.ini", LotteryMan.count.ToString(), "pwinner" + (Tprize[prize] + 1).ToString(), "") + "," + Tmember[pwinner].ToString());
                        }
                        Tprize.RemoveAt(prize);
                        Tmember.RemoveAt(pwinner);
                    }
                    result.text += "——————————";
                    LotteryMan.on = false;
                    Config.Set("Lottery.ini", "all", "on", "0");
                }
                else
                {
                    result.type = 1;
                }
            }
            else
            {
                result.text = "目前没有抽奖活动";
            }
                return result;
        } //开奖


        public FuncResult WarframeMarket(CQGroupMessageEventArgs e, string item) //wm
        {
            FuncResult result = new FuncResult();
            if (item == "") return new FuncResult(0, "参数不足！");
            item = item.Replace("，", ",");
            if (item.Contains(",")) return new FuncResult(0, WMReserve(item));
            result.text = "WM搜索结果如下：\n";
            Resp_nymph_wm jsonText = API_nymph_wm.Get(item);
            if (jsonText == null || jsonText.word == null) return new FuncResult(0, WMReserve(item));
            string item_zh = jsonText.word.zh;
            string item_en = jsonText.word.en;
            result.text += "搜索物品：" + item_zh + "\n——————————";
            int sellerNum = 3;
            if (jsonText.seller.Length < sellerNum) sellerNum = jsonText.seller.Length;
            for (int i = 0; i < sellerNum; i++)
            {
                string price = jsonText.seller[i].platinum.ToString();
                string number = jsonText.seller[i].quantity.ToString();
                string level = jsonText.seller[i].mod_rank.ToString();
                string player = jsonText.seller[i].user.ingame_name;
                result.text += "\n—价格：" + price + " —数量：" + number + " —等级：" + level;
                result.text += "\n—指令：/w " + player + " Hi! I want to buy: " + jsonText.word.search.Replace("_", " ") + " for " + price + " platinum. (warframe.market)";
                result.text += "\n——————————";
            }
            result.text += "\nurl:https://warframe.market/items/" + jsonText.word.search;
            return result;
        }
        public string WMReserve(string item) //备用wm(通过官方api)
        {
            if (!item.Contains(",")) item += ",0";
            string[] temp = item.Split(',');
            item = temp[0];
            int rank = new Regex("^[0-9]+$").IsMatch(temp[1]) ? Convert.ToInt32(temp[1]) : 1;
            string result = "WM搜索结果如下：\n";
            item = Dic.ItemHandleWM(item.Trim());
            string[] items = item.Split(',');
            foreach (string i in items)
            {
                Resp_WM jsonText = API_WM.Get(i.ToLower().Replace(" ", "_"));
                if (jsonText == null) continue;
                result += "搜索物品：" + i + "\n——————————";
                Order[] orders = jsonText.payload.orders;
                int count = orders.Length;
                int[] sortID = new int[3];
                while (!IsSatis(orders, sortID[0]))
                {
                    sortID[0]++;
                    if (sortID[0] >= orders.Length) return "未找到符合条件的商品！";
                }
                for (int j = sortID[0]; j < count; j++)
                {
                    if (IsSatis(orders, j) && orders[sortID[0]].platinum >= orders[j].platinum)
                    {
                        sortID[2] = sortID[1];
                        sortID[1] = sortID[0];
                        sortID[0] = j;
                    }
                }
                bool IsSatis(Order[] orders_, int ID_)
                {
                    if (ID_ >= orders_.Length) return false;
                    if (orders_[ID_].user.status != "ingame") return false;
                    if (orders_[ID_].order_type != "sell") return false;
                    if (orders_[ID_].platform != "pc") return false;
                    if (orders_[ID_].mod_rank < rank) return false;
                    return true;
                }
                for (int j = 0; j < sortID.Length; j++)
                {
                    string price = ((int)orders[sortID[j]].platinum).ToString();
                    string number = orders[sortID[j]].quantity.ToString();
                    string level = orders[sortID[j]].mod_rank.ToString();
                    string player = orders[sortID[j]].user.ingame_name;
                    result += "\n—价格：" + price + " —数量：" + number + " —等级：" + level;
                    result += "\n—指令：/w " + player + " Hi! I want to buy: " + i.ToLower() + " for " + price + " platinum. (warframe.market)";
                    result += "\n——————————";
                }
                result += "\nurl:https://warframe.market/items/" + i.ToLower().Replace(" ", "_");
                return result;
            }
            return "搜索物品不存在！";
        }
        public FuncResult RivenMarket(CQGroupMessageEventArgs e, string item) //rm
        {
            FuncResult result = new FuncResult();
            if (item == "") return new FuncResult(0, "参数不足！");
            result.text = "RM搜索结果如下：\n";
            Resp_nymph_rm jsonText = API_nymph_rm.Get(item);
            if (jsonText == null || jsonText.word == null) return new FuncResult(0, RMReserve(item));
            string item_zh = jsonText.word.zh;
            string item_en = jsonText.word.en;
            result.text += "搜索武器：" + item_zh + "\n——————————";
            for (int i = 0; i < 3; i++)
            {
                string price = jsonText.seller[i].price;
                string rerolls = jsonText.seller[i].rerolls;
                string mr = jsonText.seller[i].mr;
                string level = jsonText.seller[i].rank;
                string player = jsonText.seller[i].seller;
                string name = item_en + " " + jsonText.seller[i].name;
                List<string> stats = new List<string>();
                if (jsonText.seller[i].stat1 != null) stats.Add(jsonText.seller[i].stat1 + jsonText.seller[i].stat1val);
                if (jsonText.seller[i].stat2 != null) stats.Add(jsonText.seller[i].stat2 + jsonText.seller[i].stat2val);
                if (jsonText.seller[i].stat3 != null) stats.Add(jsonText.seller[i].stat3 + jsonText.seller[i].stat3val);
                if (jsonText.seller[i].stat4 != null) stats.Add(jsonText.seller[i].stat4 + jsonText.seller[i].stat4val);
                result.text += "\n—价格：" + price + " —次数：" + rerolls + " —段位：" + mr + " —等级：" + level;
                result.text += "\n—词条：|";
                for (int j = 0; j < stats.ToArray().Length; j++)
                {
                    result.text += stats[j] + "|";
                }
                result.text += "\n—指令：/w " + player + " Hey! I'd like to buy the " + name + " Riven that you sell on Riven.market for " + price + " Platinum!";
                result.text += "\n——————————";
            }
            result.text += "\nurl:https://riven.market/list/PC/" + item_en.Replace(" ", "_");
            return result;
        }
        public string RMReserve(string item) //备用rm(通过官方api)
        {
            string result = "RM搜索结果如下：\n";
            item = Dic.ItemHandleRM(item.Trim());
            string[] items = item.Split(',');
            foreach (string i in items)
            {
                string total = Net.Get(string.Format("https://riven.market/_modules/riven/showrivens.php?platform=PC&limit=3&polarity=all&rank=all&mastery=16&weapon={0}&neg=all&price=99999&rerolls=-1&sort=price&direction=ASC", i.Replace(" ", "_")));
                if (!total.Contains("div class=\"riven \"")) continue;
                List<string> orders = new List<string>();
                int site = 0;
                while (total.IndexOf("div class=\"riven \"", site) != -1)
                {
                    string temp = Sub.Mid(total, "div class=\"riven \"", "content_copy", site);
                    orders.Add(temp);
                    site = total.IndexOf("div class=\"riven \"", site) + 1;
                }
                result += "搜索武器：" + i + "\n——————————";
                for (int j = 0; j < orders.ToArray().Length; j++)
                {
                    string price = Sub.Mid(orders[j], "data-price=\"", "\"");
                    string rerolls = Sub.Mid(orders[j], "data-rerolls=\"", "\"");
                    string mr = Sub.Mid(orders[j], "data-mr=\"", "\"");
                    string level = Sub.Mid(orders[j], "data-rank=\"", "\"");
                    string player = Sub.Mid(orders[j], "profile/", "\"");
                    string name = Sub.Mid(orders[j], "data-weapon=\"", "\"") + " " + Sub.Mid(orders[j], "data-name=\"", "\"");
                    List<string> stats = new List<string>();
                    if (Sub.Mid(orders[j], "data-stat1=\"", "\"") != "") stats.Add(Sub.Mid(orders[j], "data-stat1=\"", "\"") + "+" + Sub.Mid(orders[j], "data-stat1val=\"", "\""));
                    if (Sub.Mid(orders[j], "data-stat2=\"", "\"") != "") stats.Add(Sub.Mid(orders[j], "data-stat2=\"", "\"") + "+" + Sub.Mid(orders[j], "data-stat2val=\"", "\""));
                    if (Sub.Mid(orders[j], "data-stat3=\"", "\"") != "") stats.Add(Sub.Mid(orders[j], "data-stat3=\"", "\"") + "+" + Sub.Mid(orders[j], "data-stat3val=\"", "\""));
                    if (Sub.Mid(orders[j], "data-stat4=\"", "\"") != "") stats.Add(Sub.Mid(orders[j], "data-stat4=\"", "\"") + "-" + Sub.Mid(orders[j], "data-stat4val=\"", "\""));
                    result += "\n—价格：" + price + " —次数：" + rerolls + " —段位：" + mr + " —等级：" + level;
                    result += "\n—词条：|";
                    for (int k = 0; k < stats.ToArray().Length; k++)
                    {
                        result += stats[k] + "|";
                    }
                    result += "\n—指令：/w " + player + " Hey! I'd like to buy the " + name + " Riven that you sell on Riven.market for " + price + " Platinum!";
                    result += "\n——————————";
                }
                result += "\nurl:https://riven.market/list/PC/" + i.Replace(" ", "_");
                return result;
            }
            return "搜索物品不存在！";
        }
        public FuncResult Wiki(CQGroupMessageEventArgs e, string item) //wiki
        {
            FuncResult result = new FuncResult();
            if (item == "") return new FuncResult(0, "参数不足！");
            string jsonText = Net.Get("http://nymph.rbq.life:3000/wiki/dev/" + item);
            if (Json.Get(jsonText, "total") == "")
            {
                result.text = "无搜索结果！";
                return result;
            }
            result.text = Json.Get(jsonText, "wiki", "0", "title") + "搜索结果如下：\n";
            result.text += Json.Get(jsonText, "wiki", "0", "url");
            return result;
        }
        public FuncResult LotteryAdd(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            param = param.Trim();
            if (LotteryMan.on)
            {
                if (new Regex("^[0-9]+$").IsMatch(param))
                {
                    result.text = "格式错误！\n[提供奖品 物品(,数量)]添加奖品\n例如:提供奖品 福马3个,2\n意为提供两个福马包作为奖品";
                }
                else
                {
                    param = param.Replace("，", ",");
                    string prize = "", pcount = "1";
                    if (param.Contains(","))
                    {
                        prize = param.Split(',')[0];
                        pcount = Convert.ToInt32(param.Split(',')[1]).ToString();
                    }
                    else
                    {
                        prize = param;
                    }
                    int count = Convert.ToInt32(Config.Get("Lottery.ini", LotteryMan.count.ToString(), "count", "0")) + 1;
                    Config.Set("Lottery.ini", LotteryMan.count.ToString(), "count", count.ToString());
                    Config.Set("Lottery.ini", LotteryMan.count.ToString(), "prize" + count.ToString(), prize);
                    Config.Set("Lottery.ini", LotteryMan.count.ToString(), "pcount" + count.ToString(), pcount);
                    Config.Set("Lottery.ini", LotteryMan.count.ToString(), "pmember" + count.ToString(), e.FromQQ.Id.ToString());
                    result.text = "成功添加 " + pcount + " 份 " + prize + " 作为奖品！";
                }
            }
            else
            {
                result.text = "目前没有抽奖活动";
            }
            return result;
        } //提供奖品
        public FuncResult MonitorAdd(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (e.FromQQ != Config.qqAdmin && !MemberMan.IsAdmin(e) && !MemberMan.IsCreator(e)) return new FuncResult(1);
            param = param.Trim();
            if (param == "") return new FuncResult(0, "参数不足！");
            if (Monitor.keywords.Contains(param)) return new FuncResult(0, "关键词已存在！");
            string keywords = Config.Get("monitor.ini", "all", "keywords", "");
            Config.Set("monitor.ini", "all", "keywords", keywords == "" ? param : keywords + "," + param);
            Monitor.keywords.Add(param);
            result.text = "添加关键词 [" + param + "] 成功！";
            return result;
        } //添加监控
        public FuncResult MonitorRemove(CQGroupMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (e.FromQQ != Config.qqAdmin && !MemberMan.IsAdmin(e) && !MemberMan.IsCreator(e)) return new FuncResult(1);
            param = param.Trim();
            if (param == "") return new FuncResult(0, "参数不足！");
            string keywords = Config.Get("monitor.ini", "all", "keywords", "");
            string temp = "";
            foreach (string i in keywords.Split(','))
            {
                if (i != param) temp += i + ",";
            }
            if (temp.Length != 0) temp = temp.Substring(0, temp.Length - 1);
            if (temp != keywords)
            {
                Config.Set("monitor.ini", "all", "keywords", temp);
                Monitor.keywords.Remove(param);
                result.text = "删除关键词 [" + param + "] 成功！";
            }
            else
            {
                result.text = "关键词不存在！";
            }
            return result;
        } //删除监控



        public FuncResult Search(CQPrivateMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (param == "") return new FuncResult(0, "参数不足！\n[查询 关键词]\n多个关键词之间可用空格分割");
            result.text = "——————————";
            int count = 0;
            string[] keywords = param.ToLower().Split(' ');
            foreach (KeyValuePair<string, string> i in Dic.dict)
            {
                if (Sub.IsContain(i.Key.ToLower(), keywords) || Sub.IsContain(i.Value.ToLower(), keywords))
                {
                    result.text += "\n—" + i.Key + "\n—" + i.Value + "\n——————————";
                    count++;
                }
            }
            if (count == 0)
            {
                result.text = "未找到相关内容！";
            }
            else
            { 
                result.text = "共找到 " + count.ToString() + " 条词条\n" + result.text;
            }
            if (result.text.Length > 3420) result.text = "数据过多,请缩小范围！";
            return result;
        } //词库查询
        public FuncResult Translate(CQPrivateMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (param == "") return new FuncResult(0, "参数不足！");
            Resp_Translate Info = API.JsonToClass<Resp_Translate>(@"http://fanyi.youdao.com/translate?&doctype=json&type=AUTO&i=" + param);
            if (Info == null || Info.translateResult.Length == 0)
            {
                result.text = "获取出错！请稍后重试或联系物理管理员";
            }
            else
            {
                result.text = Info.translateResult[0][0].tgt;
            }
            return result;
        } //翻译
        public FuncResult Relic(CQPrivateMessageEventArgs e, string param)
        {
            FuncResult result = new FuncResult();
            if (param == "") return new FuncResult(0, "参数不足！");
            result.text = "——————————";
            if (param.Contains("纪"))
            {
                foreach (Relic i in Dic.relics)
                {
                    if (i.name.ToLower() == param.ToLower())
                    {
                        result.text += "\n—" + i.item_1;
                        result.text += "\n—" + i.item_2;
                        result.text += "\n—" + i.item_3;
                        result.text += "\n—" + i.item_4;
                        result.text += "\n—" + i.item_5;
                        result.text += "\n—" + i.item_6;
                        result.text += "\n——————————";
                        return result;
                    }
                }
                result.text = "未找到此遗物！";
            }
            else
            {
                string[] keys = param.ToLower().Split(' ');
                bool flag = false;
                foreach (Relic i in Dic.relics)
                {
                    if (IsValid(i, keys))
                    {
                        result.text += "\n—" + i.name;
                        result.text += "[" + (i.baro ? "奸商携带" : i.vault ? "已入库" : "未入库") + "]";
                        if (!flag) flag = true;
                    }
                }
                if (flag) result.text += "\n——————————";
                else result.text = "未找到符合要求的遗物！";
                bool IsValid(Relic relic, string[] keywords)
                {
                    bool valid = true;
                    foreach (string i in keywords)
                    {
                        if (!relic.item_1.ToLower().Contains(i)) valid = false;
                    }
                    if (valid) return true;
                    valid = true;
                    foreach (string i in keywords)
                    {
                        if (!relic.item_2.ToLower().Contains(i)) valid = false;
                    }
                    if (valid) return true;
                    valid = true;
                    foreach (string i in keywords)
                    {
                        if (!relic.item_3.ToLower().Contains(i)) valid = false;
                    }
                    if (valid) return true;
                    valid = true;
                    foreach (string i in keywords)
                    {
                        if (!relic.item_4.ToLower().Contains(i)) valid = false;
                    }
                    if (valid) return true;
                    valid = true;
                    foreach (string i in keywords)
                    {
                        if (!relic.item_5.ToLower().Contains(i)) valid = false;
                    }
                    if (valid) return true;
                    valid = true;
                    foreach (string i in keywords)
                    {
                        if (!relic.item_6.ToLower().Contains(i)) valid = false;
                    }
                    return valid;
                }
            }
            return result;
        } //遗物


        public string TimeHandle(TimeSpan time)
        {
            string result = "";
            if (time.Days != 0) result += time.Days + " 天 ";
            if (time.Hours != 0) result += time.Hours + " 时 ";
            if (time.Minutes != 0) result += time.Minutes + " 分 ";
            result += time.Seconds + " 秒";
            return result;
        }
    }
}
