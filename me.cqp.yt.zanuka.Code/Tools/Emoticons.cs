using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.yt.zanuka.Code
{
    public static class Emoticons
    {
        public static List<string> files = new List<string>();
        public static int Get() //获取image文件夹下的所有表情包
        {
            string path = Config.cjPath.Replace(Sub.Mid(Config.cjPath + " ", @"data\", " "), @"image\");
            DirectoryInfo root = new DirectoryInfo(path);
            files.Clear();
            foreach (FileInfo f in root.GetFiles())
            {
                if (f.Name.Substring(0, 3) == "emo") files.Add(f.Name);
            }
            return files.ToArray().Length;
        }
    }
}
