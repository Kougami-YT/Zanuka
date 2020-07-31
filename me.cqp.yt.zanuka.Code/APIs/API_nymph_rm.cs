using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using json.nymph_rm;
using Newtonsoft.Json;

namespace me.cqp.yt.zanuka.Code
{
    class API_nymph_rm
    {
        public static Resp_nymph_rm Get(string item)
        {
            string url = @"http://nymph.rbq.life:3000/rm/dev/" + item;
            try
            {
                return JsonConvert.DeserializeObject<Resp_nymph_rm>(JsonConvert.SerializeObject(HttpHelper.GetAPI<Resp_nymph_rm>(url)));
            }
            catch
            {
                return null;
            }
        }
    }
}
