using Proto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Libs.Utility;

namespace MasterServer
{
    class ServerRegHandle:XNet.Libs.Net.MessageHandler
    {
        public override void Handle(XNet.Libs.Net.Message message, XNet.Libs.Net.Client client)
        {
            byte[] result = message.Content;
            RegServer reg = new RegServer();
            using(var mem = new MemoryStream(result))
            {
                using(var br = new BinaryReader(mem))
                {
                    reg.ParseFormBinary(br);
                }
            }
           
             Debuger.Log(string.Format("Server:{0} Reg. Listen:{1}:{2} MaxClient:{3}",client.Socket.RemoteEndPoint,reg.ListenIP,reg.Port,reg.MaxClient));
        }
    }
}
