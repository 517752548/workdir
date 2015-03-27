using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNet.Libs.Net;

namespace StopMasterServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var ip = args[0];
            var prot = Convert.ToInt32(args[1]);
            var client = new XNet.Libs.Net.SocketClient(prot, ip);
            client.UseSendThreadUpdate = true;
            client.OnConnectCompleted = (s, e) =>
            {
                client.SendMessage(new XNet.Libs.Net.Message(MessageClass.Request, 100, new byte[] { 1 }));
                OK = true;
            };
            client.Connect();
            while (!OK)
            {
                Thread.Sleep(1000);
                if(OK)
                {
                    client.Disconnect();
                }
            }
        }

        private static bool OK = false;
    }
}
