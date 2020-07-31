using json.WM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.yt.zanuka.Code
{
    class API_WM
    {
        public static Resp_WM Get(string item)
        {
            string url = @"https://api.warframe.market/v1/items/" + item + "/orders";
            try
            {
                return JsonConvert.DeserializeObject<Resp_WM>(Net.Get(url));
            }
            catch
            {
                return null;
            }
        }
    }
}
