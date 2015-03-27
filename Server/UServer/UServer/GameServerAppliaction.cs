using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UServer
{
    class GameServerAppliaction
    {
        public void Start()
        {
            var connectManager = new XNet.Libs.Net.ConnectionManager();
            Server = new XNet.Libs.Net.SocketServer(connectManager, 9000);
            Server.Start();
        }


        public XNet.Libs.Net.SocketServer Server { private set; get; }

        private DateTime lastTime;
        public void Tick()
        {
            if ((DateTime.Now - lastTime).TotalSeconds > 3)
            {
                lastTime = DateTime.Now;
                Console.WriteLine(string.Format("Client:{0}", Server.CurrentConnectionManager.Count));
                Server.CurrentConnectionManager.Each((client) =>
                {
                    if ((DateTime.Now - client.LastMessageTime).TotalSeconds > 10)
                    {
                        Server.DisConnectClient(client, 1);
                    }
                });
            }
        }
        public void Exit()
        {
            Server.Stop();
        }

        
    }
}
