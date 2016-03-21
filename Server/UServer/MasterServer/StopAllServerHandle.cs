using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer
{
    class StopAllServerHandle:XNet.Libs.Net.MessageHandler
    {
        public override void Handle(XNet.Libs.Net.Message message, XNet.Libs.Net.Client client)
        {

            Program.Stop();
        }
    }
}
