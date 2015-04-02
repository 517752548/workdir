/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Proto
{
    public enum UserType
    {
        /// <summary>
        /// 
        /// </summary>
        Normal=1,
        /// <summary>
        /// 
        /// </summary>
        Default=2,

    }
    public enum HandlerNo
    {
        /// <summary>
        /// 注册服务器
        /// </summary>
        RegServer=10,
        /// <summary>
        /// 关闭中心服务器
        /// </summary>
        StopMasterServer=100,
        /// <summary>
        /// 处理网络消息的handler
        /// </summary>
        MessageHandler=120,

    }
}