using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer
{
    class ServerRegHandle:XNet.Libs.Net.MessageHandler
    {
        public override void Handle(XNet.Libs.Net.Message message, XNet.Libs.Net.Client client)
        {
            XNet.Libs.Utility.Debuger.Log(string.Format("Server:{0} Reg.",client.Socket.RemoteEndPoint));
        }
    }
}
