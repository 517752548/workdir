using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UServer
{
    class Program
    {
        static void Main(string[] args)
        {

            server = new GameServerAppliaction();
            server.Start(Convert.ToInt32(args[0]));

            mainThread = new Thread(new ThreadStart(Worker));
            IsRunning = true;
            mainThread.IsBackground = true;
            mainThread.Start();
            while(true)
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
