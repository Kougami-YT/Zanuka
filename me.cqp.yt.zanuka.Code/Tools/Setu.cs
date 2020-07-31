using Native.Sdk.Cqp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace me.cqp.yt.zanuka.Code
{
    public static class Setu
    {
        public static List<string> files = new List<string>();
        public static int GetLocal() //获取image文件夹下的所有涩图
        {
            string path = Config.cjPath.Replace(Sub.Mid(Config.cjPath + " ", @"data\", " "), @"image\");
            DirectoryInfo root = new DirectoryInfo(path);
            files.Clear();
            foreach (FileInfo f in root.GetFiles())
            {
                if (f.Name.Substring(0, 4) == "setu") files.Add(f.Name);
            }
            return files.ToArray().Length;
        }
        public static async void GetOnline(int ac) //下载konachan上的涩图
        {
            string[] konachan = {
                "http://konachan.wjcodes.com/index.php?tag=yuri",
                "http://konachan.wjcodes.com/index.php?tag=loli",
                "http://konachan.wjcodes.com/index.php?tag=flat_chest",
                "http://konachan.wjcodes.com/index.php?tag=2girls",
                "http://konachan.wjcodes.com/index.php?tag=game_cg",
                "http://konachan.wjcodes.com/index.php?tag=masturbation",
                "http://konachan.wjcodes.com/index.php?tag=vibrator",

                "http://konachan.wjcodes.com/index.php?tag=yuri+loli",
                "http://konachan.wjcodes.com/index.php?tag=yuri+flat_chest",
                "http://konachan.wjcodes.com/index.php?tag=yuri+2girls",
                "http://konachan.wjcodes.com/index.php?tag=yuri+game_cg",
                "http://konachan.wjcodes.com/index.php?tag=yuri+kiss",
                "http://konachan.wjcodes.com/index.php?tag=yuri+pussy",
                "http://konachan.wjcodes.com/index.php?tag=yuri+sex",
                "http://konachan.wjcodes.com/index.php?tag=yuri+masturbation",
                "http://konachan.wjcodes.com/index.php?tag=yuri+vibrator",

                "http://konachan.wjcodes.com/index.php?tag=loli+flat_chest",
                "http://konachan.wjcodes.com/index.php?tag=loli+2girls",
                "http://konachan.wjcodes.com/index.php?tag=loli+game_cg",
                "http://konachan.wjcodes.com/index.php?tag=loli+masturbation",
                "http://konachan.wjcodes.com/index.php?tag=loli+vibrator",

                "http://konachan.wjcodes.com/index.php?tag=flat_chest+2girls",
                "http://konachan.wjcodes.com/index.php?tag=flat_chest+game_cg",
                "http://konachan.wjcodes.com/index.php?tag=flat_chest+masturbation",
                "http://konachan.wjcodes.com/index.php?tag=flat_chest+vibrator",

                "http://konachan.wjcodes.com/index.php?tag=2girls+game_cg",
                "http://konachan.wjcodes.com/index.php?tag=2girls+masturbation",
                "http://konachan.wjcodes.com/index.php?tag=2girls+vibrator",

                "http://konachan.wjcodes.com/index.php?tag=game_cg+masturbation",
                "http://konachan.wjcodes.com/index.php?tag=game_cg+vibrator",

                "http://konachan.wjcodes.com/index.php?tag=masturbation+vibrator"
            };
            List<string> urls = new List<string>();
            for (int i = 0; i < konachan.Length; i++)
            {
                string text = Net.Get(konachan[i]);
                int site = 1, count = 0;
                while (site != 0)
                {
                    string url = Sub.Mid(text, @"ajaxs('", @"'", site);
                    if (url != "*nothing*" && !urls.Contains(url)) urls.Add(url.Replace("konachan.net", "konachan.com"));
                    site = text.IndexOf(@"ajaxs('", site) + 1;
                    count += 1;
                }
            }
            int countL = GetLocal();
            for (int i = 0; i < countL; i++) //删除不完整图片
            {
                if (!IsCompletedImage(Config.cjPath.Replace(Sub.Mid(Config.cjPath + " ", @"data\", " "), @"image\" + files[i])))
                {
                    File.Delete(Config.cjPath.Replace(Sub.Mid(Config.cjPath + " ", @"data\", " "), @"image\" + files[i]));
                    new CQLog(ac).Debug("涩图", "删除不完整图片" + files[i]);
                }
            }
            for (int i = 0; i < urls.ToArray().Length; i++) //下载新图片
            {
                bool flag = false;
                for (int j = 0; j < countL; j++)
                {
                    if (urls[i].Contains(Sub.Mid(files[j], "setu", ".jpg")))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    while (countDownload >= 10) Thread.Sleep(1000);
                    string imgID = Sub.Mid(urls[i], "Konachan.com%20-%20", "%20");
                    Download(urls[i], Config.cjPath.Replace(Sub.Mid(Config.cjPath + " ", @"data\", " "), @"image\setu" + imgID + ".jpg"), ac);
                    //new WebClient().DownloadFile(new Uri(urls[i]), Config.cjPath.Replace(Sub.Mid(Config.cjPath + " ", @"data\", " "), @"image\setu" + imgID + ".jpg"));
                    new CQLog(ac).Debug("涩图", "开始下载" + imgID);
                }
            }
            await Task.Run(() =>
            {
                GetOnline(ac);
            });
        }
        public static bool IsCompletedImage(string strFileName)
        {
            try
            {
                FileStream fs = new FileStream(strFileName, FileMode.Open);
                BinaryReader reader = new BinaryReader(fs);
                try
                {
                    byte[] szBuffer = reader.ReadBytes((int)fs.Length);
                    //jpg png图是根据最前面和最后面特殊字节确定. bmp根据文件长度确定
                    //png检查
                    if (szBuffer[0] == 137 && szBuffer[1] == 80 && szBuffer[2] == 78 && szBuffer[3] == 71 && szBuffer[4] == 13
                        && szBuffer[5] == 10 && szBuffer[6] == 26 && szBuffer[7] == 10)
                    {
                        //&& szBuffer[szBuffer.Length - 8] == 73 && szBuffer[szBuffer.Length - 7] == 69 && szBuffer[szBuffer.Length - 6] == 78
                        if (szBuffer[szBuffer.Length - 5] == 68 && szBuffer[szBuffer.Length - 4] == 174 && szBuffer[szBuffer.Length - 3] == 66
                            && szBuffer[szBuffer.Length - 2] == 96 && szBuffer[szBuffer.Length - 1] == 130)
                            return true;
                        //有些情况最后多了些没用的字节
                        for (int i = szBuffer.Length - 1; i > szBuffer.Length / 2; --i)
                        {
                            if (szBuffer[i - 5] == 68 && szBuffer[i - 4] == 174 && szBuffer[i - 3] == 66
                             && szBuffer[i - 2] == 96 && szBuffer[i - 1] == 130)
                                return true;
                        }


                    }
                    else if (szBuffer[0] == 66 && szBuffer[1] == 77)//bmp
                    {
                        //bmp长度
                        //整数转成字符串拼接
                        string str = Convert.ToString(szBuffer[5], 16) + Convert.ToString(szBuffer[4], 16)
                            + Convert.ToString(szBuffer[3], 16) + Convert.ToString(szBuffer[2], 16);
                        int iLength = Convert.ToInt32("0x" + str, 16); //16进制数转成整数
                        if (iLength <= szBuffer.Length) //有些图比实际要长
                            return true;
                    }
                    else if (szBuffer[0] == 71 && szBuffer[1] == 73 && szBuffer[2] == 70 && szBuffer[3] == 56)//gif
                    {
                        //标准gif 检查00 3B
                        if (szBuffer[szBuffer.Length - 2] == 0 && szBuffer[szBuffer.Length - 1] == 59)
                            return true;
                        //检查含00 3B
                        for (int i = szBuffer.Length - 1; i > szBuffer.Length / 2; --i)
                        {
                            if (szBuffer[i] != 0)
                            {
                                if (szBuffer[i] == 59 && szBuffer[i - 1] == 0)
                                    return true;
                            }
                        }
                    }
                    else if (szBuffer[0] == 255 && szBuffer[1] == 216) //jpg
                    {
                        //标准jpeg最后出现ff d9
                        if (szBuffer[szBuffer.Length - 2] == 255 && szBuffer[szBuffer.Length - 1] == 217)
                            return true;
                        else
                        {
                            //有好多jpg最后被人为补了些字符也能打得开, 算作完整jpg, ffd9出现在近末端
                            //jpeg开始几个是特殊字节, 所以最后大于10就行了 从最后字符遍历
                            //有些文件会出现两个ffd9 后半部分ffd9才行
                            for (int i = szBuffer.Length - 2; i > szBuffer.Length / 2; --i)
                            {
                                //检查有没有ffd9连在一起的
                                if (szBuffer[i] == 255 && szBuffer[i + 1] == 217)
                                    return true;
                            }
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                    if (reader != null)
                        reader.Close();
                }
            }
            catch
            {
                return false;
            }
            return false;
        } //判断图片是否完整
        public static int countDownload = 0;
        public static async void Download(string url, string path, int ac) //异步下载
        {
            countDownload += 1;
            await Task.Run(() =>
            {
                WebClient wc = new WebClient();
                //Task task = wc.DownloadFileTaskAsync(url, path);
                //task.Wait();
                wc.DownloadFileAsync(new Uri(url), path);
                while (wc.IsBusy) Thread.Sleep(1000);
            });
            new CQLog(ac).Debug("涩图", "成功下载" + Sub.Mid(url, "Konachan.com%20-%20", "%20"));
            countDownload -= 1;
        }
    }
}
