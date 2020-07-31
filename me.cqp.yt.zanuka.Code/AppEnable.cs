using System.IO;
using me.cqp.yt.zanuka.Code.Tools;
using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;

namespace me.cqp.yt.zanuka.Code
{
    public class Event_AppEnable : IAppEnable
    {
        /// <summary>
        /// 插件初始化
        /// </summary>
        /// <param name="sender">事件来源对象</param>
        /// <param name="e">附加的事件参数</param>
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            Config.cjPath = e.CQApi.AppDirectory; //插件目录读取
            Config.qqAdmin = long.Parse(Config.Get("default.ini", "all", "admin", "1329623049")); //物理管理员读取
            GroupMan.Get(); //管理群读取
            if (Config.Get("default.ini", "all", "dict", "0") == "0") //词库加载
            {
                Dic.GetOnline();
            }
            else
            {
                Dic.GetLocal();
            }
            Monitor.Start(e); //监控
            //Setu.GetOnline(e.CQLog.AuthCode); //下载涩图
        }
    }
}
