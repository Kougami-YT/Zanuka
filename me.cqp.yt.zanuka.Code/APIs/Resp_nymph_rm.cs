using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace json.nymph_rm
{
    public class Resp_nymph_rm
    {
        public int page { get; set; }
        public int size { get; set; }
        public int total { get; set; }
        public Word word { get; set; }
        public object[] words { get; set; }
        public Seller[] seller { get; set; }
    }

    public class Word
    {
        public int id { get; set; }
        public string type { get; set; }
        public string zh { get; set; }
        public string en { get; set; }
    }

    public class Seller
    {
        public string id { get; set; }
        public string weapon { get; set; }
        public string wType { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string age { get; set; }
        public string rank { get; set; }
        public string mr { get; set; }
        public string rerolls { get; set; }
        public string polarity { get; set; }
        public string stat1 { get; set; }
        public string stat1val { get; set; }
        public string stat2 { get; set; }
        public string stat2val { get; set; }
        public string stat3 { get; set; }
        public string stat3val { get; set; }
        public string stat4 { get; set; }
        public string stat4val { get; set; }
        public string seller { get; set; }
        public string status { get; set; }
    }

}
