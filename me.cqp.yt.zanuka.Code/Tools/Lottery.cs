using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.yt.zanuka.Code
{
    public static class LotteryMan
    {
        public static bool on = Config.Get("Lottery.ini", "all", "on", "0") == "1" ? true : false;
        public static int count = Convert.ToInt32(Config.Get("Lottery.ini", "all", "count", "0"));
    }
}
