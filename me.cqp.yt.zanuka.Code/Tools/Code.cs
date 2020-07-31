using Native.Sdk.Cqp;
using System;

namespace me.cqp.yt.zanuka.Code
{
    public static class Code
    {
        public static string Pic(string url) //图片码
        {
            return CQApi.CQCode_Image(url).ToString();
        }
        public static string At(long qq) //@码
        {
            if (qq == 0) return CQApi.CQCode_AtAll().ToString();
            return CQApi.CQCode_At(qq).ToString();
        }
    }
}
