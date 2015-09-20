using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Libs.Net;
using XNet.Libs.Utility;

namespace MasterServer
{
    class ReportServerStatusHandle :MessageHandler
    {
        public override void Handle(Message message, Client client)
        {
            var status = new Proto.ReportServerStatus();
            using(var mem = new MemoryStream(message.Content))
            {
                 using(var br = new BinaryReader(mem))
                 {
                     status.ParseFormBinary(br);
                 }
            }

            Debuger.Log(string.Format("Server:{2} client:{0} max:{1}", status.CurrentClient,status.MaxClient, client.Socket.RemoteEndPoint));
        }
    }
}
