using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MasterServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //1000 
            var port = Convert.ToInt32(args[0]);

            Server = new XNet.Libs.Net.SocketServer(new XNet.Libs.Net.ConnectionManager(), port);
            HanderManager = new XNet.Libs.Net.DefaultMessageHandlerManager();
            Server.HandlerManager = HanderManager;
            HanderManager.RegsiterHandler((int)Proto.HandlerNo.RegServer, typeof(ServerRegHandle));
            HanderManager.RegsiterHandler((int)Proto.HandlerNo.StopMasterServer, typeof(StopAllServerHandle));
            Server.Start();
            while (Server.IsWorking)
            {
                Thread.Sleep(1000);

            }
        }

        private static XNet.Libs.Net.DefaultMessageHandlerManager HanderManager;

        public static void Stop()
        {
            if(Server.IsWorking)
            {
                Server.Stop();
            }
        }

        private static XNet.Libs.Net.SocketServer Server;
    }
}
