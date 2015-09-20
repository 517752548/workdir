using Proto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Libs.Net;
using XNet.Libs.Utility;

namespace UServer
{
    class GameServerAppliaction
    {
        public void Start(string listenIP,int port, string MasterServerIp, int masterServerProt, int maxClient)
        {
            var handler = new MessageHandler();
            MaxClient = maxClient;
            SessionClient = new SocketClient(masterServerProt, MasterServerIp);
            SessionClient.UseSendThreadUpdate = true;
            SessionClient.OnConnectCompleted = (s, e) =>
            {
                if (!e.Success)
                {
                    Debuger.LogError(string.Format("Can't Conent masterServer:{0}:{1}",MasterServerIp,masterServerProt));
                    Program.IsRunning = false;
                }
                var reg = new RegServer { ListenIP = listenIP, Port = port, MaxClient = maxClient  };
                //send message
                byte[] bytes=null;
                using(var mem = new MemoryStream())
                {
                    using(var bw = new BinaryWriter(mem))
                    {
                        reg.ToBinary(bw);
                    }
                    bytes = mem.ToArray();
                }
                SessionClient.SendMessage(new XNet.Libs.Net.Message(MessageClass.Request,
                    (int)Proto.HandlerNo.RegServer, bytes));
            };
            SessionClient.OnPingCompleted = (s, e) =>
            {
                var detal = e.DelayMillisecond;
                //XNet.Libs.Utility.Debuger.Log(string.Format("Delay:{0:0.0}ms", detal));
            };

            SessionClient.OnDisconnect = (s, e) =>
            {
                Program.IsRunning = false;
            };
            SessionClient.Connect();
            var connectManager = new XNet.Libs.Net.ConnectionManager();
            DefaultHandler = new XNet.Libs.Net.DefaultMessageHandlerManager();
            DefaultHandler.RegsiterHandler((int)Proto.HandlerNo.MessageHandler, typeof(MessageHandler));
            Server = new XNet.Libs.Net.SocketServer(connectManager, port);
            Server.HandlerManager = DefaultHandler;
            Server.Start();

        }



        private XNet.Libs.Net.DefaultMessageHandlerManager DefaultHandler;

        public XNet.Libs.Net.SocketServer Server { private set; get; }

        private DateTime lastTime;
        public void Tick()
        {
            if ((DateTime.Now - lastTime).TotalSeconds > 3)
            {
                lastTime = DateTime.Now;
                //Debuger.Log (string.Format("Client:{0}", Server.CurrentConnectionManager.Count));
                Server.CurrentConnectionManager.Each((client) =>
                {
                    if ((DateTime.Now - client.LastMessageTime).TotalSeconds > 10)
                    {
                        Server.DisConnectClient(client, 1);
                    }
                });

                if (SessionClient != null && SessionClient.IsConnect)
                {
                    var report = new ReportServerStatus { CurrentClient = Server.CurrentConnectionManager.Count, MaxClient = MaxClient };
                    //send message
                    byte[] bytes = null;
                    using (var mem = new MemoryStream())
                    {
                        using (var bw = new BinaryWriter(mem))
                        {
                            report.ToBinary(bw);
                        }
                        bytes = mem.ToArray();
                    }
                    SessionClient.SendMessage(new XNet.Libs.Net.Message(MessageClass.Request,
                        (int)Proto.HandlerNo.ReportStatus, bytes));
                }
            }
        }

       
        public void Exit()
        {
            Server.Stop();
        }

        public SocketClient SessionClient { private set; get; }

        public int MaxClient { get; set; }
    }
}
