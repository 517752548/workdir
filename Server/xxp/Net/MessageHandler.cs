﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XNet.Libs.Net
{
    
    /// <summary>
    /// 消息处理抽象
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public abstract class MessageHandler
    {
        /// <summary>
        /// 处理一个消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="client"></param>
        public abstract void Handle(Message message, Client client);
    }

    /// <summary>
    /// 消息处理管理抽象
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public abstract class MessageHandlerManager
    {
        public abstract void Handle(Message netMessage, Client client);
    }

    /// <summary>
    /// 普通的消息处理管理
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public class DefaultMessageHandlerManager : MessageHandlerManager
    {
        private Dictionary<int, Type> Handlers = new Dictionary<int, Type>();
        /// <summary>
        /// 注册一个消息处理者
        /// </summary>
        /// <param name="listenMessageNo"></param>
        /// <param name="handlerClass"></param>
        public void RegsiterHandler(int listenMessageNo, Type handlerClass)
        {
            if (Handlers.ContainsKey(listenMessageNo))
            {
                throw new ExistHandlerException(listenMessageNo);
            }

            Handlers.Add(listenMessageNo, handlerClass);

        }

        public  override void Handle(Message netMessage, Client client)
        {
            int no = netMessage.Flag;
            if (Handlers.ContainsKey(no))
            {
                var handler = Activator.CreateInstance(Handlers[no]);
                var method = Handlers[no].GetMethod("Handle");
                method.Invoke(handler, new object[]{ netMessage, client});
            }
            else
            {
                Utility.Debuger.LogWaring(string.Format("No handle Message NO:{0}", no));
            }
        }
    }

}
