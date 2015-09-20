﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace XNet.Libs.Net
{
	/// <summary>
	/// 连接客户端
	/// @author:xxp
	/// @date:2013/01/10
	/// </summary>
	public class SocketClient
	{
		/// <summary>
		/// 服务器端口
		/// </summary>
		public int Port { set; get; }
		/// <summary>
		/// 服务器IP
		/// </summary>
		public string IP { set; get; }

		private Socket _socket;

		private Dictionary<byte, ServerMessageHandler> Handlers { set; get; }
		/// <summary>
		/// 注册一个消息处理者
		/// </summary>
		/// <param name="type"></param>
		/// <param name="handler"></param>
		public void RegisterHandler (byte type, ServerMessageHandler handler)
		{
			if (Handlers.ContainsKey (type)) {
				throw new ExistHandlerException (type);
			}

			Handlers.Add (type, handler);
			handler.Connection = this;
		}

		/// <summary>
		/// 初始化
		/// </summary>
		public virtual void Init ()
		{
		}

		/// <summary>
		/// 获取处理者
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public ServerMessageHandler GetHandler (byte type)
		{
			ServerMessageHandler handler;
			Handlers.TryGetValue (type, out handler);
			return handler;
		}

		public SocketClient (int port, string ip)
		{
			Port = port;
			IP = ip;
			Stream = new MessageStream ();
			Handlers = new Dictionary<byte, ServerMessageHandler> ();
			BufferMessage = new MessageQueue<Message> ();
			ReceiveBufferMessage = new MessageQueue<Message> ();
			Init ();
		}

		/// <summary>
		/// 连接
		/// </summary>
		public void Connect ()
		{
			if (IsConnect) {
				throw new Exception ("Is connecting!");
			}
			_socket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_socket.SendTimeout = 999;
			_socket.ReceiveTimeout = 999;

			var connectArgs = new SocketAsyncEventArgs ();
			connectArgs.RemoteEndPoint = new IPEndPoint (IPAddress.Parse (IP), Port);
			connectArgs.Completed += connectArgs_Completed;
			_socket.ConnectAsync (connectArgs);
		}

		void connectArgs_Completed (object sender, SocketAsyncEventArgs e)
		{

			IsConnect = _socket.Connected;
			if (IsConnect) {
				byte[] response = new byte[1024];
				e.SetBuffer (response, 0, response.Length);
				_socket.ReceiveAsync (e);
				e.Completed -= connectArgs_Completed;
				e.Completed += received_Completed;
				ProcessThread = new Thread (new ThreadStart (UpdateProcess));
				ProcessThread.IsBackground = true;
				ProcessThread.Start ();
			}
			OnConnect (IsConnect);
		}

		void received_Completed (object sender, SocketAsyncEventArgs e)
		{
			this.Stream.Write (e.Buffer, e.Offset, e.BytesTransferred);
			Message message;
			while (Stream.Read(out message)) {
				this.OnReceived (message);
			}
			if (IsConnect)
				_socket.ReceiveAsync (e);
		}

		private Thread ProcessThread;

		private MessageStream Stream { set; get; }
		/// <summary>
		/// 当前连接状态
		/// </summary>
		public bool IsConnect { private set; get; }
		/// <summary>
		/// 连接成功后调用
		/// </summary>
		/// <param name="isSuccess"></param>
		public virtual void OnConnect (bool isSuccess)
		{
			if (OnConnectCompleted != null) {
				OnConnectCompleted (this, new ConnectCommpletedArgs { Success = isSuccess });
			}
		}


		/// <summary>
		/// 接收到消息
		/// </summary>
		/// <param name="message"></param>
		public virtual void OnReceived (Message message)
		{
			if (message.Class == (byte)MessageClass.Ping) {
				#region Ping
				if (!IsPinging)
					return;
				var tickNow = DateTime.Now.Ticks;
				long tickSend = 0;
				using (var mem = new MemoryStream(message.Content)) {
					using (var br = new BinaryReader(mem)) {
						tickSend = br.ReadInt64 ();
					}
				}
				Delay = (tickNow - tickSend);
				IsPinging = false;
				if (OnPingCompleted != null) {
					OnPingCompleted (this, new PingCompletedArgs { DelayTicks = Delay });
				}
				#endregion
			} else if (message.Class == (byte)MessageClass.Package) {
				var bufferPackage = MessageBufferPackage.ParseFromMessage (message);
				foreach (var m in bufferPackage.Messages) {
					OnReceived (m);
				}
			} else if (message.Class == (byte)MessageClass.Close) {
				this.Disconnect ();
			} else {
				ReceivedBufferMessage (message);
			}
		}

		private void ReceivedBufferMessage (Message message)
		{
			ReceiveBufferMessage.AddMessage (message);
		}

		/// <summary>
		/// 处理消息
		/// </summary>
		/// <param name="message"></param>
		private void HandleMessage (Message message)
		{
			if (Handlers.ContainsKey (message.Class)) {
				Handlers [message.Class].Handle (message);
			} else {
				Utility.Debuger.DebugLog ("No handle Message!");
			}
		}



		/// <summary>
		/// 刷新
		/// </summary>
		public virtual void Update ()
		{
			UpdateHandle ();
		}

		private long LastPingTime = 0;

		/// <summary>
		/// Ping 的间隔时间 毫秒
		/// </summary>
		public int PingDurtion = 3000; //

		/// <summary>
		/// 发送一个消息
		/// </summary>
		/// <param name="message"></param>
		public void SendMessage (Message message)
		{
			BufferMessage.AddMessage (message);
		}

		private bool DoSend (Message message)
		{
			try {
				if (_socket.Connected) {
					SocketAsyncEventArgs myMsg = new SocketAsyncEventArgs ();
					myMsg.RemoteEndPoint = _socket.RemoteEndPoint;
					byte[] buffer = message.ToBytes ();
					myMsg.SetBuffer (buffer, 0, buffer.Length);
					_socket.SendAsync (myMsg);
					return true;
				} else {
					HandleException (new Exception ("Disconnected"));
					return false;
				}
			} catch (Exception ex) {
				HandleException (ex);
				return false;
			}
		}
		/// <summary>
		/// 处理错误
		/// </summary>
		/// <param name="ex"></param>
		public void HandleException (Exception ex)
		{
			Disconnect ();
			Utility.Debuger.DebugLog ("Client Disconnect:" + ex);
		}
		/// <summary>
		/// 断开连接
		/// </summary>
		public void Disconnect ()
		{
			if (this.IsConnect) {
				IsConnect = false;
				if (this.ProcessThread != null) {
					ProcessThread.Join ();
				}
				Close ();
				if (OnDisconnect != null) {
					OnDisconnect (this, new EventArgs ());
				}
			}
		}

		private void Close ()
		{
			this._socket.Shutdown (SocketShutdown.Both);
			this._socket.Close ();
			this._socket = null;
			XNet.Libs.Utility.Debuger.DebugLog ("Client socket Disconnect!");
		}
		/// <summary>
		/// 连接完成
		/// </summary>
		public ConnectCallBack OnConnectCompleted;

		private void UpdateProcess ()
		{
			while (IsConnect) {
				//发送数据
				var messages = BufferMessage.GetMessage ();
				if (messages != null && messages.Count > 0) {
					lastBufferMessageSize = messages.Count;
					var buffer = new MessageBufferPackage ();
					while (messages.Count > 0) {
						buffer.AddMessage (messages.Dequeue ());
					}
					DoSend (buffer.ToMessage ());
					messages.Clear ();
				}
				if (UseSendThreadUpdate)
					Update();

				Thread.Sleep (1);
			}
		}

		private void UpdateHandle()
		{
			//处理Handle
			var received = ReceiveBufferMessage.GetMessage ();
			if (received != null && received.Count > 0) {
				while (received.Count > 0) {
					HandleMessage (received.Dequeue ());
				}
			}
			//time out 
			if (LastPingTime + PingDurtion * TimeSpan.TicksPerMillisecond < DateTime.Now.Ticks) {
				LastPingTime = DateTime.Now.Ticks;
				Ping ();
			}
			foreach (var handler in Handlers.Values)
				handler.Update ();
            
		}

		public bool UseSendThreadUpdate = false;
		
		private MessageQueue<Message> BufferMessage { set; get; }
		/// <summary>
		/// 网络延迟
		/// </summary>
		public long Delay { private set; get; }

		private bool IsPinging = false;
		/// <summary>
		/// Ping 服务器
		/// </summary>
		public void Ping ()
		{
			if (IsPinging)
				return;
			IsPinging = true;
			long tick = DateTime.Now.Ticks;
			byte[] bytes;
			using (var mem = new MemoryStream()) {
				using (var bw = new BinaryWriter(mem)) {
					bw.Write (tick);
				}
				bytes = mem.ToArray ();
			}
			var message = new Message (MessageClass.Ping, 0, bytes);
			SendMessage (message);
		}
		/// <summary>
		/// Ping 完成事件
		/// </summary>
		public PingCallBack OnPingCompleted;

		/// <summary>
		/// 当前发送包的数量
		/// </summary>
		public volatile int lastBufferMessageSize;

		private MessageQueue<Message> ReceiveBufferMessage { set; get; }

		/// <summary>
		/// 断开连接
		/// </summary>
		public DisconnectCallBack OnDisconnect;

		public delegate void PingCallBack(object sender,PingCompletedArgs args);
		public delegate void ConnectCallBack(object sender, ConnectCommpletedArgs args);

		public delegate void DisconnectCallBack(object sender, EventArgs args);
	}


	/// <summary>
	/// 消息处理抽象类
	/// @author:xxp
	/// @date:2013/01/10
	/// </summary>
	public abstract class ServerMessageHandler
	{
		/// <summary>
		/// 处理一个消息
		/// </summary>
		/// <param name="message"></param>
		public abstract void Handle (Message message);
		/// <summary>
		/// 更行
		/// </summary>
		public virtual void Update ()
		{
		}
		/// <summary>
		/// 当前的连接
		/// </summary>
		public SocketClient Connection { set; get; }

	}
	/// <summary>
	/// 连接成功参数
	/// </summary>
	public class ConnectCommpletedArgs : EventArgs
	{
		/// <summary>
		/// 成功与否
		/// </summary>
		public bool Success { set; get; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class PingCompletedArgs : EventArgs
	{
		/// <summary>
		/// ticks
		/// </summary>
		public long DelayTicks { set; get; }
		/// <summary>
		/// Millisecond
		/// </summary>
		public double DelayMillisecond {
			get {
				return DelayTicks / TimeSpan.TicksPerMillisecond;
			}
		}
	}
}
