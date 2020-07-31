using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace me.cqp.yt.zanuka.Code
{
    public static class Json
    {
        public static string Get(string jsonText, params string[] paras) //取json数据
        {
            if (paras.Length == 0)
            {
                return jsonText;
            }
            else
            {
                try
                {
                    JToken result = JToken.Parse(jsonText);
                    for (int i = 0; i < paras.Length; i++)
                    {
                        try
                        {
                            if (IsNumeric(paras[i]))
                            {
                                result = result[Convert.ToInt32(paras[i])];
                            }
                            else
                            {
                                if (Exist(result.ToString(), paras[i]))
                                {
                                    result = result[paras[i]];
                                }
                                else
                                {
                                    return "";
                                }

                            }
                        }
                        catch
                        {
                            return "";
                        }
                    }
                    return result.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
        public static int Count(string jsonText) //取json成员数
        {
            JToken json = JToken.Parse(jsonText);
            return json.Count();
        }
        public static bool Exist(string jsonText, string key) //判断键是否存在
        {
            JObject json = JObject.Parse(jsonText);
            if (json.Property(key) != null) return true;
            return false;
        }
        static bool IsNumeric(string str) //判断一段字符串是否为数字
        {
            if (str == null || str.Length == 0)    //验证这个参数是否为空
                return false;                           //是，就返回False
            ASCIIEncoding ascii = new ASCIIEncoding();//new ASCIIEncoding 的实例
            byte[] bytestr = ascii.GetBytes(str);         //把string类型的参数保存到数组里

            foreach (byte c in bytestr)                   //遍历这个数组里的内容
            {
                if (c < 48 || c > 57)                          //判断是否为数字
                {
                    return false;                              //不是，就返回False
                }
            }
            return true;                                        //是，就返回True
        }
    }
}
