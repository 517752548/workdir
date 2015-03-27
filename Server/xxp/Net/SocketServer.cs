﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using XNet.Libs.Utility;

namespace XNet.Libs.Net
{
    /// <summary>
    /// Tcp 服务器
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public class SocketServer
    {
        /// <summary>
        /// 服务端连接
        /// </summary>
        /// <param name="client"></param>
        public virtual void OnConnect(Client client) { }
        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public virtual void OnReceivedMessag(Client client, Message msg)
        {
			client.LastMessageTime = DateTime.UtcNow;
            if (msg.Class == (byte)MessageClass.Ping)
            {
                SendMessage(client, msg);
            }
            else if (msg.Class == (byte)MessageClass.Package)
            {
                var package = MessageBufferPackage.ParseFromMessage(msg);
                foreach (var i in package.Messages)
                {
                    OnReceivedMessag(client, i);
                }
            }
            else if (this.HandlerManager != null)
            {
                this.HandlerManager.Handle(msg, client);
            }
            else
            {
                Utility.Debuger.DebugLog("Server No Handler!");
            }
        }
        /// <summary>
        /// 关闭一个连接
        /// </summary>
        /// <param name="client"></param>
        public virtual void CloseConnection(Client client) { }
        public virtual void Init() { }
        /// <summary>
        /// 当前连接管理
        /// </summary>
        public ConnectionManager CurrentConnectionManager { set; get; }
        public int Port { set; get; }
        /// <summary>
        /// 消息处理管理
        /// </summary>
        public MessageHandlerManager HandlerManager { set; get; }
        public SocketServer(ConnectionManager cManager, int port)
        {
            CurrentConnectionManager = cManager;
            Port = port;
            BufferQueue = new MessageQueue<SendMessageBuffer>();
            Init();
        }
        /// <summary>
        /// 消息最大字节数
        /// </summary>
        public static int MaxReceiveSize = 1024 * 1024;
        private ManualResetEvent MREvent = new ManualResetEvent(false);
        private Socket _socket;

        public void Start()
        {
            IsWorking = true;
            CurrentConnectionManager.Clear();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.LingerState = new LingerOption(true, 10);
            _socket.Bind(new IPEndPoint(IPAddress.Any, Port));
            _socket.Listen(2000);
            AcceptThread = new Thread(new ThreadStart(ThreadRun));
            AcceptThread.IsBackground = true;
            AcceptThread.Start();
            SendThread = new Thread(new ThreadStart(SendMessage));
            SendThread.IsBackground = true;
            SendThread.Start();
        }

        private Thread SendThread;

        private Thread AcceptThread;

        private void ThreadRun()
        {
            while (IsWorking)
            {
                MREvent.Reset();
                _socket.BeginAccept(new AsyncCallback(OnAccpet), _socket);
                MREvent.WaitOne();
            }
        }

        private void OnAccpet(IAsyncResult ar)
        {
            MREvent.Set();
            if (IsWorking)
            {
                Socket server = (Socket)ar.AsyncState;
                Socket client = server.EndAccept(ar);
                if (client != null)
                {
                    var nClient = CurrentConnectionManager.CreateClient(client, this);
                    try
                    {
                        nClient.Socket.BeginReceive(nClient.Buffer, 0,
                            nClient.Buffer.Length,
                            SocketFlags.None,
                            new AsyncCallback(OnDataReceived), nClient);
                        OnConnect(nClient);
                    }
                    catch (Exception ex)
                    {
                        HandleException(nClient, ex);
                    }
                }
            }
        }

        private void OnDataReceived(IAsyncResult ar)
        {
            var nClient = ar.AsyncState as Client;
            int count = 0;
            try
            {
                if (nClient.IsClose) return;
                SocketError errorCode;
                count = nClient.Socket.EndReceive(ar, out errorCode);
                if (count > 0)
                {
                    nClient.Stream.Write(nClient.Buffer, 0, count);
                }
                Message message;
                while (nClient.Stream.Read(out message))
                {
                    if (MaxReceiveSize == 0 || (message.Size < MaxReceiveSize))
                    {
                        //int flag = message.Flag;
                        OnReceivedMessag(nClient, message);

                    }
                    else
                    {
                        HandleException(nClient, new Exception("Max Receive Size limit"));
                    }
                }
                nClient.Socket.BeginReceive(nClient.Buffer, 0,
                        nClient.Buffer.Length,
                        SocketFlags.None,
                        new AsyncCallback(OnDataReceived), nClient);
            }
            catch (Exception ex)
            {
                HandleException(nClient, ex);
            }
        }

        private void OnSentData(IAsyncResult ar)
        {
            var client = ar.AsyncState as Client;
            try
            {
                if (client.IsClose) return;
                client.Socket.EndSend(ar);
            }
            catch (Exception ex)
            {
                HandleException(client, ex);
            }
        }

        /// <summary>
        /// 服务器是否启动
        /// </summary>
        public volatile bool IsWorking;
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (IsWorking)
            {
                Utility.Debuger.DebugLog("Socket Server:Stoping....");
                CurrentConnectionManager.Each((client) =>
                {
                    //服务器关闭
                    DisConnectClient(client, 0);
                });
                CurrentConnectionManager.Clear();

                IsWorking = false;
                MREvent.Set();
                AcceptThread.Join();
                SendThread.Join();
                SendThread = null;
                AcceptThread = null;
                _socket.Close();
                _socket = null;
               

            }
        }

        private void SendMessage(Client client, byte[] msg)
        {
            try
            {
                if (client.IsClose) return;
                client.Socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, new AsyncCallback(OnSentData), client);
            }
            catch (Exception ex)
            {
                HandleException(client, ex);
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="msg"></param>
        public void SendMessage(Client client, Message msg)
        {
            BufferQueue.AddMessage(new SendMessageBuffer(client, msg));
        }

        private void HandleException(Client client, Exception ex)
        {

            Utility.Debuger.DebugLog(ex.Message);
            RemoveClient(client);

        }

        private void RemoveClient(Client client)
        {
            client.Close();
            CurrentConnectionManager.RemoveClient(client);
            CloseConnection(client);
        }

        private void SendMessage()
        {
            while (IsWorking)
            {
                var queue = (BufferQueue.GetMessage());
                if (queue != null && queue.Count > 0)
                {
                    lastBufferSize = queue.Count;
                    var dir = new Dictionary<Client, MessageBufferPackage>();
                    foreach (var i in queue)
                    {
                        if (dir.ContainsKey(i.Client))
                        {
                            dir[i.Client].AddMessage(i.Message);
                        }
                        else
                        {
                            dir.Add(i.Client, new MessageBufferPackage(i.Message));
                        }
                    }
                    foreach (var i in dir)
                    {
                        SendMessage(i.Key, i.Value.ToMessage().ToBytes());
                    }
                    queue.Clear();
                }

                Thread.Sleep(35);
            }

        }

        /// <summary>
        /// 发送缓存
        /// </summary>
        private MessageQueue<SendMessageBuffer> BufferQueue { set; get; }

        /// <summary>
        /// 当前消息数量
        /// </summary>
        public volatile int lastBufferSize;


        /// <summary>
        /// 关闭一个会话
        /// </summary>
        /// <param name="client"></param>
        /// <param name="code">关闭的时候显示的错误代码</param>
        public void DisConnectClient(Client client, byte code)
        {
			try{
            //发送一个关闭信息
            if(client.Socket.Connected)
			  this.SendMessage ( client,new Message(MessageClass.Close, 0, new byte[] { code }).ToBytes());
			}catch(Exception ex)
			{
				Debuger.LogError(ex);
			}
            RemoveClient(client);
        }
    }


    /// <summary>
    /// 发送消息的缓存
    /// </summary>
    public class SendMessageBuffer
    {
        public SendMessageBuffer(Net.Client client, Net.Message msg)
        {
            // TODO: Complete member initialization
            this.Client = client;
            this.Message = msg;
        }
        public Net.Client Client { set; get; }
        public Message Message { set; get; }
    }

}
