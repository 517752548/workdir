using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XNet.Libs.Net
{
	/// <summary>
	/// 请求管理类，用来管理对服务器发送请求，并处理服务器返回响应
	/// @author:xxp
	/// @date:2013/01/10
	/// </summary>
	public class RequestResponseHandlerManager : ServerMessageHandler
	{
		public override void Handle (Message message)
		{
			var result = new ResponserResult ();
			result.ParseFormBinary (message.Content);
			RequesterClient requester;
			lock (Syncroot) {
				Requesters.TryGetValue (result.RequesterID, out requester);
			}
			if (requester != null) {
				CompleteRequester (requester, ResponseType.Success, result);
			}
			DoListener (result);
		}

		public override void Update ()
		{ 
			base.Update ();
			if (!this.Connection.IsConnect) {
				foreach (var request in AllRequesters) {
					this.CompleteRequester (request, ResponseType.ServerError, new ResponserResult
				                       {
										API = request.Request.API,
										Result = string.Empty,
										RequesterID = request.ID,
										Success = false
										});
				}
			} else {
				double milsecond = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
				if (lastCheck + 1000 > milsecond)
					return;
				lastCheck = milsecond;
				foreach (var i in AllRequesters) {
					if (i.TimeOut * 1000 + i.BeginTime < milsecond) {
						CompleteRequester (i, ResponseType.TimeOut, new ResponserResult { RequesterID = i.ID, Result = string.Empty, Success = false });
					}
				}
				lastCheck = milsecond;
			}
		}

		private double lastCheck { set; get; }

		public RequestResponseHandlerManager ()
		{
		}

		private Dictionary<long, RequesterClient> Requesters = new Dictionary<long, RequesterClient> ();
		private object Syncroot = new object ();

		/// <summary>
		/// 完成请求
		/// </summary>
		/// <param name="requester"></param>
		/// <param name="type"></param>
		/// <param name="result"></param>
		public void CompleteRequester (RequesterClient requester, ResponseType type, ResponserResult result)
		{
			lock (Syncroot) {
				if (Requesters.Remove (requester.ID)) {
				}
			}
			requester.RequestResponseType = type;
			requester.Completed (result, type);
		}
		/// <summary>
		/// 当前尚未有响应的请求
		/// </summary>
		public List<RequesterClient> AllRequesters {
			get {
				lock (Syncroot) {
					return Requesters.Values.ToList ();
				}
			}
		}
		/// <summary>
		/// 发送一个请求到服务器
		/// </summary>
		/// <param name="request"></param>
		public void SendRequest (RequesterClient request)
		{
			lock (Syncroot) {
				//重置
				if (_id == int.MaxValue) {
					_id = 0;
				}
				request.ID = _id++;
				RequesterCount++;
				request.BeginTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
				Requesters.Add (request.ID, request);
			}
			var bytes = request.Request.ToBinary ();
			var message = new Message ((byte)MessageClass.Request, 0, bytes);
			if (this.Connection.IsConnect)
				this.Connection.SendMessage (message);
		}

		private  int _id = 0;
		/// <summary>
		/// 根据 API 创建一个请求
		/// </summary>
		/// <param name="api"></param>
		/// <returns></returns>
		public RequesterClient CreateRequest (string api)
		{
			var requester = new RequesterClient (this, api);
			return requester;
		}

		/// <summary>
		/// 当前队列中的请求
		/// </summary>
		public int RealRequesterCount {
			get {
				lock (Syncroot) {
					return Requesters.Count;
				}
			}
		}

		/// <summary>
		/// 请求的数量
		/// </summary>
		public volatile int RequesterCount = 0;
		private Dictionary<string, ClientResponseListener> Listeners = new Dictionary<string, ClientResponseListener> ();

		/// <summary>
		/// 注册一个消息处理监听
		/// </summary>
		/// <param name="api"></param>
		/// <param name="listener"></param>
		public void RegsiterListener (string api, ClientResponseListener listener)
		{
			if (!Listeners.ContainsKey (api)) {
				Listeners.Add (api, listener);
			} else {
				throw new Exception ("listener exists!");
			}
		}

		private void DoListener (ResponserResult result)
		{
			foreach (var i in Listeners) {
				if (i.Key.Equals (result.API, StringComparison.OrdinalIgnoreCase)) {
					i.Value.ProcessResponse (result);
				}
			}
		}

		/// <summary>
		/// 注册一个程序集的监听者
		/// </summary>
		/// <param name="assmbly"></param>
		public void RegsiterListeners (Assembly assmbly)
		{
			var listeners = assmbly.GetTypes ().Where (t => t.BaseType == typeof(Xxp.Libs.Net.ClientResponseListener));
			foreach (var listener in listeners) {
				var att = listener.GetCustomAttributes (typeof(Xxp.Libs.Net.ClientRquesterListenerAPI), false) [0] 
                    as Xxp.Libs.Net.ClientRquesterListenerAPI;
				if (att == null)
					continue;
				RegsiterListener (att.API, Activator.CreateInstance (listener) as ClientResponseListener);
			}
		}
	}

	/// <summary>
	/// 请求
	/// 用于创建对服务器发送一个相应式消息
	/// @author:xxp
	/// @date:2013/01/10
	/// </summary>
	public class RequesterClient
	{
		public RequesterClient (RequestResponseHandlerManager manager, string Api)
		{
			Manager = manager;
			Request = new Request ();
			Request.API = Api;
			TimeOut = 30;//s
			RequestResponseType = ResponseType.NoResponse;
		}

		private long _id = -1;
		/// <summary>
		/// 请求参数
		/// </summary>
		public Request Request { set; get; }
		/// <summary>
		/// 当前ID
		/// </summary>
		public long ID {
			set {
				_id = value;
				Request.RequesterID = _id;
			}
			get {
				return _id;
			}
		}
		/// <summary>
		/// 发送请求到服务器
		/// </summary>
		public void SendRequest ()
		{
			Manager.SendRequest (this);
		}
		/// <summary>
		/// 请求得到响应后调用的事件
		/// </summary>
		public  CompletedCallBack OnCompleted;
		/// <summary>
		/// 完成这个请求
		/// </summary>
		/// <param name="result"></param>
		/// <param name="type"></param>
		public void Completed (ResponserResult result, ResponseType type)
		{
			if (OnCompleted != null) {
				OnCompleted (this, new RequestCompletedArgs { Result = result, Type = type });
			}
		}
		/// <summary>
		/// 超时时间 
		/// s
		/// </summary>
		public int TimeOut { set; get; }
		/// <summary>
		/// 开始请求时间
		/// </summary>
		public double BeginTime { set; get; }
		/// <summary>
		/// 请求的管理对象
		/// </summary>
		public RequestResponseHandlerManager Manager { set; get; }
		/// <summary>
		/// 返回响应类型
		/// </summary>
		public ResponseType RequestResponseType { set; get; } 

		public delegate void CompletedCallBack(object sender,RequestCompletedArgs args);
	}

	/// <summary>
	/// 请求响应后的结果
	/// @author:xxp
	/// @date:2013/01/10
	/// </summary>
	public class RequestCompletedArgs : EventArgs
	{
		public ResponserResult Result { set; get; }

		public ResponseType Type { set; get; }
	}
	/// <summary>
	/// 响应结果
	/// </summary>
	public enum ResponseType
	{
		/// <summary>
		/// 没有响应
		/// </summary>
		NoResponse,
		/// <summary>
		/// 超时
		/// </summary>
		TimeOut,
		/// <summary>
		/// 服务器错误
		/// </summary>
		ServerError,
		/// <summary>
		/// 成功
		/// </summary>
		Success
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class ClientRquesterListenerAPI : Attribute
	{
		public string API { set; get; }

		public ClientRquesterListenerAPI (string api)
		{
			API = api; 
		}
	}

	public abstract class ClientResponseListener
	{
		/// <summary>
		/// 处理响应 
		/// </summary>
		/// <param name="result"></param>
		public abstract void ProcessResponse (ResponserResult result);  
	}
}
