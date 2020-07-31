using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using WarframeStat_Class;

namespace me.cqp.yt.zanuka.Code
{
    public static class API
    {
        public static T JsonToClass<T>(string url)
        {
            string json = Net.Get(url);
            if (string.IsNullOrEmpty(json)) json = "[]";
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }
        public static string url_Cetus = "https://api.warframestat.us/pc/cetusCycle";
        public static string url_Earth = "https://api.warframestat.us/pc/earthCycle";
        public static string url_Vallis = "https://api.warframestat.us/pc/vallisCycle";
        public static string url_Arbitration = "https://api.warframestat.us/pc/arbitration";
        public static string url_Sortie = "https://api.warframestat.us/pc/sortie";
        public static string url_Invasions = "https://api.warframestat.us/pc/invasions";
        public static string url_Fissures = "https://api.warframestat.us/pc/fissures";
        public static string url_DailyDeals = "https://api.warframestat.us/pc/dailyDeals";
        public static string url_VoidTrader = "https://api.warframestat.us/pc/voidTrader";
        public static string url_Nightwave = "https://api.warframestat.us/pc/nightwave";

        public static T XmlToClass<T>(string xml) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
