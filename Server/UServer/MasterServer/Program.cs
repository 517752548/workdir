using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNet.Libs.Utility;

namespace MasterServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //1000 
            var port = Convert.ToInt32(args[0]);
            var stopProt = Convert.ToInt32(args[1]);

            Server = new XNet.Libs.Net.SocketServer(new XNet.Libs.Net.ConnectionManager(), port);
            HanderManager = new XNet.Libs.Net.DefaultMessageHandlerManager();
            Server.HandlerManager = HanderManager;
            HanderManager.RegsiterHandler((int)Proto.HandlerNo.RegServer, typeof(ServerRegHandle));
            HanderManager.RegsiterHandler((int)Proto.HandlerNo.ReportStatus, typeof(ReportServerStatusHandle));
            Debuger.Log(string.Format("MasterServer listen port {0}", port));
            Server.Start();

            StopListenServer = new XNet.Libs.Net.SocketServer(new XNet.Libs.Net.ConnectionManager(), stopProt);
            var tempHandler = new XNet.Libs.Net.DefaultMessageHandlerManager();
            StopListenServer.HandlerManager = tempHandler;
            //tempHandler.RegsiterHandler((int)Proto.HandlerNo.RegServer, typeof(ServerRegHandle));
            tempHandler.RegsiterHandler((int)Proto.HandlerNo.StopMasterServer, typeof(StopAllServerHandle));
            Debuger.Log(string.Format("MasterServer Stop port {0}", stopProt));
            StopListenServer.Start();
            while (Server.IsWorking)
            {
                Thread.Sleep(1000);
            }
        }

        private static XNet.Libs.Net.DefaultMessageHandlerManager HanderManager;

        public static void Stop()
        {
            if (Server.IsWorking)
            {
                Server.Stop();
            }
            if(StopListenServer.IsWorking)
                StopListenServer.Stop();
        }

        private static XNet.Libs.Net.SocketServer Server;
        private static XNet.Libs.Net.SocketServer StopListenServer;
    }
}
