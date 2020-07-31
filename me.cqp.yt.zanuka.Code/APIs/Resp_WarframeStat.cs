using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarframeStat_Class
{

    public class Resp_Cetus
    {
        public string id { get; set; }
        public DateTime expiry { get; set; }
        public DateTime activation { get; set; }
        public bool isDay { get; set; }
        public string state { get; set; }
        public string timeLeft { get; set; }
        public bool isCetus { get; set; }
        public string shortString { get; set; }
    }

    public class Resp_Earth
    {
        public string id { get; set; }
        public DateTime expiry { get; set; }
        public DateTime activation { get; set; }
        public bool isDay { get; set; }
        public string state { get; set; }
        public string timeLeft { get; set; }
    }

    public class Resp_Arbitration
    {
        public DateTime activation { get; set; }
        public DateTime expiry { get; set; }
        public string enemy { get; set; }
        public string type { get; set; }
        public bool archwing { get; set; }
        public bool sharkwing { get; set; }
        public string node { get; set; }
    }

    public class Resp_Vallis
    {
        public string id { get; set; }
        public DateTime expiry { get; set; }
        public bool isWarm { get; set; }
        public string state { get; set; }
        public DateTime activation { get; set; }
        public string timeLeft { get; set; }
        public string shortString { get; set; }
    }

    public class Resp_Sortie
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public DateTime expiry { get; set; }
        public bool active { get; set; }
        public string rewardPool { get; set; }
        public Variant[] variants { get; set; }
        public string boss { get; set; }
        public string faction { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
    }
    public class Variant
    {
        public string boss { get; set; }
        public string planet { get; set; }
        public string missionType { get; set; }
        public string modifier { get; set; }
        public string modifierDescription { get; set; }
        public string node { get; set; }
    }

    public class Resp_Invasions
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public string node { get; set; }
        public string desc { get; set; }
        public Attackerreward attackerReward { get; set; }
        public string attackingFaction { get; set; }
        public Defenderreward defenderReward { get; set; }
        public string defendingFaction { get; set; }
        public bool vsInfestation { get; set; }
        public int count { get; set; }
        public int requiredRuns { get; set; }
        public float completion { get; set; }
        public bool completed { get; set; }
        public string eta { get; set; }
        public string[] rewardTypes { get; set; }
    }
    public class Attackerreward
    {
        public object[] items { get; set; }
        public Counteditem[] countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }
    public class Counteditem
    {
        public int count { get; set; }
        public string type { get; set; }
        public string key { get; set; }
    }
    public class Defenderreward
    {
        public object[] items { get; set; }
        public Counteditem1[] countedItems { get; set; }
        public int credits { get; set; }
        public string asString { get; set; }
        public string itemString { get; set; }
        public string thumbnail { get; set; }
        public int color { get; set; }
    }
    public class Counteditem1
    {
        public int count { get; set; }
        public string type { get; set; }
        public string key { get; set; }
    }

    public class Resp_Fissures
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public DateTime expiry { get; set; }
        public bool active { get; set; }
        public string node { get; set; }
        public string missionType { get; set; }
        public string enemy { get; set; }
        public string tier { get; set; }
        public int tierNum { get; set; }
        public bool expired { get; set; }
        public string eta { get; set; }
    }

    public class Resp_DailyDeals
    {
        public string item { get; set; }
        public DateTime expiry { get; set; }
        public DateTime activation { get; set; }
        public int originalPrice { get; set; }
        public int salePrice { get; set; }
        public int total { get; set; }
        public int sold { get; set; }
        public string id { get; set; }
        public string eta { get; set; }
        public int discount { get; set; }
    }

    public class Resp_VoidTrader
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public DateTime expiry { get; set; }
        public bool active { get; set; }
        public string character { get; set; }
        public string location { get; set; }
        public object[] inventory { get; set; }
        public string psId { get; set; }
        public string endString { get; set; }
    }

    public class Resp_Nightwave
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public DateTime expiry { get; set; }
        public bool active { get; set; }
        public int season { get; set; }
        public string tag { get; set; }
        public int phase { get; set; }
        public Params _params { get; set; }
        public object[] possibleChallenges { get; set; }
        public Activechallenge[] activeChallenges { get; set; }
        public string[] rewardTypes { get; set; }
    }
    public class Params
    {
        public int ctc { get; set; }
    }
    public class Activechallenge
    {
        public string id { get; set; }
        public DateTime activation { get; set; }
        public string startString { get; set; }
        public DateTime expiry { get; set; }
        public bool active { get; set; }
        public bool isDaily { get; set; }
        public bool isElite { get; set; }
        public string desc { get; set; }
        public string title { get; set; }
        public int reputation { get; set; }
    }

    public class Resp_Translate
{
        public string type { get; set; }
        public int errorCode { get; set; }
        public int elapsedTime { get; set; }
        public Translateresult[][] translateResult { get; set; }
    }
    public class Translateresult
    {
        public string src { get; set; }
        public string tgt { get; set; }
    }



    

}
