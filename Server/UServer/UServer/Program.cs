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

			var listenIP = "127.0.0.1";
			var port =1201;
			var masterServerIP="127.0.0.1";
			var masterServerPort = 1200;
			var maxClient = 100;

			if (args.Length > 0) {
			
				listenIP = args [0];
				port = Convert.ToInt32 (args [1]);
				masterServerIP = args [2];
				masterServerPort = Convert.ToInt32 (args [3]);
				maxClient = Convert.ToInt32 (args [4]);

			}
            server = new GameServerAppliaction();
            
            Debuger.Log(string.Format("Listen:{3}:{0} MasterServer:{1}:{2} MaxClient:{4}",
				port, masterServerIP, masterServerPort,listenIP,maxClient));
			server.Start(listenIP,port,masterServerIP,masterServerPort,maxClient);

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
