using System;

namespace json.WM
{

    public class Resp_WM
    {
        public Payload payload { get; set; }
    }

    public class Payload
    {
        public Order[] orders { get; set; }
    }

    public class Order
    {
        public int quantity { get; set; }
        public DateTime last_update { get; set; }
        public bool visible { get; set; }
        public int mod_rank { get; set; }
        public User user { get; set; }
        public float platinum { get; set; }
        public DateTime creation_date { get; set; }
        public string region { get; set; }
        public string order_type { get; set; }
        public string platform { get; set; }
        public string id { get; set; }
    }

    public class User
    {
        public string ingame_name { get; set; }
        public DateTime? last_seen { get; set; }
        public int reputation_bonus { get; set; }
        public int reputation { get; set; }
        public string region { get; set; }
        public string avatar { get; set; }
        public string status { get; set; }
        public string id { get; set; }
    }

}
