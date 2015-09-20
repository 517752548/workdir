using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNet.Libs.Utility;

namespace UServer
{
    class Program
    {
        static void Main(string[] args)
        {

            server = new GameServerAppliaction();
            var listenIP = args[0];
            var port = Convert.ToInt32(args[1]);
            var masterServerIP = args[2];
            var masterServerProt = Convert.ToInt32(args[3]);
            var maxClient = Convert.ToInt32(args[4]);
            Debuger.Log(string.Format("Listen:{3}:{0} MasterServer:{1}:{2} MaxClient:{4}", port, masterServerIP, masterServerProt,listenIP,maxClient));
            server.Start(listenIP,port,masterServerIP,masterServerProt,maxClient);

            mainThread = new Thread(new ThreadStart(Worker));
            IsRunning = true;
            mainThread.IsBackground = true;
            mainThread.Start();
            while (IsRunning)
            {
                Thread.Sleep(10);
            }
        }

        

        private static Thread mainThread;

        public static bool IsRunning = false;

        private static GameServerAppliaction server;
        private static void Worker()
        {
           while(IsRunning)
           {
               Thread.Sleep(1);
               server.Tick();
           }

           server.Exit();
        }
    }
}
