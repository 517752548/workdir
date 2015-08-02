/************************************************/
//本代码自动生成，切勿手动修改
/************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace Proto
{
    public enum GameVersion
    {
        /// <summary>
        /// 主要版本
        /// </summary>
        Master=1,
        /// <summary>
        /// 更新版本
        /// </summary>
        Major=0,
        /// <summary>
        /// 开发版本
        /// </summary>
        Develop=1,
        /// <summary>
        /// 资源版本
        /// </summary>
        Resources=0,

    }
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