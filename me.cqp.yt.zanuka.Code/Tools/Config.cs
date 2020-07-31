using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace me.cqp.yt.zanuka.Code
{
    public static class Config
    {
        public static string cjPath = "";
        public static long qqAdmin = 0;
        static readonly string confCode = "gb2312";
        public static void Write(string file, string text) //写文件
        {
            string[] temps = text.Split('\n');
            using (StreamWriter sw = new StreamWriter(file, false, Encoding.GetEncoding(confCode)))
            {
                foreach (string s in temps)
                {
                    sw.WriteLine(s);
                }
            }
        }
        public static string Read(string file) //读文件
        {
            if (!File.Exists(file))
            {
                return "";
            }
            StringBuilder temp = new StringBuilder();
            string line = "";
            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding(confCode)))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    temp.Append(line + "\n");
                }
            }
            string result = temp.ToString();
            result = TrimE(result);
            return result;
        }
        public static string TrimE(string text) //去除多余换行
        {
            while (text.Substring(text.Length - 1, 1) == "\n")
            {
                text = text.Substring(0, text.Length - 1);
            }
            return text;
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        public static void Set(string file, string section, string key, string value) //写配置项
        {
            WritePrivateProfileString(section, key, value, cjPath + file);
        }
        public static string Get(string file, string section, string key, string def = "") //读配置项
        {
            StringBuilder result = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, result, 1024, cjPath + file);
            return result.ToString();
        }
    }
    public static class Reread
    {
        public static string lastR = "";
        public static string last = "";
    }
}
