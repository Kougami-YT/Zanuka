using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace json.nymph_wm
{

    public class Resp_nymph_wm
    {
        public int page { get; set; }
        public int size { get; set; }
        public Word word { get; set; }
        public object[] words { get; set; }
        public Statistics statistics { get; set; }
        public Seller[] seller { get; set; }
    }

    public class Word
    {
        public int id { get; set; }
        public string type { get; set; }
        public string search { get; set; }
        public string zh { get; set; }
        public string en { get; set; }
    }

    public class Statistics
    {
        public DateTime datetime { get; set; }
        public int volume { get; set; }
        public int min_price { get; set; }
        public int max_price { get; set; }
        public float avg_price { get; set; }
        public float wa_price { get; set; }
        public int median { get; set; }
        public string order_type { get; set; }
        public int mod_rank { get; set; }
        public string id { get; set; }
    }

    public class Seller
    {
        public int platinum { get; set; }
        public int quantity { get; set; }
        public string order_type { get; set; }
        public int mod_rank { get; set; }
        public User user { get; set; }
        public string platform { get; set; }
        public string region { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime last_update { get; set; }
        public bool visible { get; set; }
        public string id { get; set; }
    }

    public class User
    {
        public int reputation { get; set; }
        public int reputation_bonus { get; set; }
        public string region { get; set; }
        public DateTime last_seen { get; set; }
        public string ingame_name { get; set; }
        public string status { get; set; }
        public string id { get; set; }
        public string avatar { get; set; }
    }

}
