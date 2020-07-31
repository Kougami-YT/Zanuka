using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using json.nymph_wm;
using Newtonsoft.Json;

namespace me.cqp.yt.zanuka.Code
{
    class API_nymph_wm
    {
        public static Resp_nymph_wm Get(string item)
        {
            string url = @"http://nymph.rbq.life:3000/wm/dev/" + item;
            try
            {
                return JsonConvert.DeserializeObject<Resp_nymph_wm>(JsonConvert.SerializeObject(HttpHelper.GetAPI<Resp_nymph_wm>(url)));
            }
            catch
            {
                return null;
            }
        }
    }
}
