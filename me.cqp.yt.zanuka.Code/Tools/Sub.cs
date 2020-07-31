using System;

namespace me.cqp.yt.zanuka.Code
{
    public static class Sub
    {
        public static string Mid(string source, string left, string right, int position = 0) //取两端字符串中间的字符串
        {
            int start, end;
            string result;
            source = source.Substring(position, source.Length - position);
            start = source.IndexOf(left) + left.Length;
            if (start == -1 + left.Length)
            {
                return "*nothing*";
            }
            source = source.Substring(start, source.Length - start);
            end = source.IndexOf(right);
            if (end == -1)
            {
                return "*nothing*";
            }
            result = source.Substring(0, end);
            return result;
        }
        public static void Sep(string total, out string order, out string param) //分离指令与参数
        {
            string[] temp = total.Split(' ');
            order = temp[0];
            if (temp.Length == 1)
            {
                param = "";
            }
            else
            {
                param = total.Substring((order + " ").Length);
            }
        }
        public static bool IsContain(string text, string[] keywords)
        {
            foreach (string i in keywords)
            {
                if (!text.Contains(i)) return false;
            }
            return true;
        } //判断一段文本中是否同时存在多个关键词
    }
}
