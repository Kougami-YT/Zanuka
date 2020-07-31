using System;

namespace me.cqp.yt.zanuka.Code
{
    public static class Ran
    {
        public static Random rnd = new Random();
        public static int Int(int max) //生成随机数
        {
            return rnd.Next(max);
        }
        public static int Int(int min, int max) //生成随机数
        {
            return rnd.Next(min, max);
        }
    }
}
