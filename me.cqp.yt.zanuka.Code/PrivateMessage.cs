using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.yt.zanuka.Code
{
    /// <summary>
    /// 私聊消息处理
    /// </summary>
    /// <param name="sender">事件来源对象</param>
    /// <param name="e">附加的事件参数</param>
    public class Event_PrivateMessage : IPrivateMessage
    {
        public void PrivateMessage(object sender, CQPrivateMessageEventArgs e)
        {
            string msg = e.Message.Text;

            Sub.Sep(msg, out string order, out string param); //分割指令名与参数
            foreach (KeyValuePair<string, Function.FuncP> i in instruct)
            {
                if (i.Key == order.ToLower())
                {
                    FuncResult result = i.Value(e, param);
                    switch (result.type)
                    {
                        case 0:
                            e.FromQQ.SendPrivateMessage(result.text);
                            break;
                        case 1:
                            e.FromQQ.SendPrivateMessage("权限不足！");
                            break;
                    }
                }
            }

            
            // 设置该属性, 表示阻塞本条消息, 该属性会在方法结束后传递给酷Q
            e.Handler = true;
        }

        static readonly Dictionary<string, Function.FuncP> instruct = new Dictionary<string, Function.FuncP>() //指令字典 无参数
        {
            { "查询" , new Function.FuncP(new Function().Search)},
            { "翻译" , new Function.FuncP(new Function().Translate)},
            { "遗物" , new Function.FuncP(new Function().Relic)},
        };


    }
}
