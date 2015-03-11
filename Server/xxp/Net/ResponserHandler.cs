using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace XNet.Libs.Net
{
    /// <summary>
    /// 响应请求 消息处理者
    /// @author:xxp
    /// @date:2013/01/10
    /// </summary>
    public class ResponserHandler : MessageHandler
    {
        public override void Handle(Message message, Client client)
        {

            var request = new Request();
            request.ParseFormBinary(message.Content);

            if (Manager != null)
            {
                Manager.DoReponse(request, client);
            }
        }
        /// <summary>
        /// 响应 管理
        /// </summary>
        public ResponserManager Manager { set; get; }

        public ResponserHandler(ResponserManager manager)
        {
            Manager = manager;
        }
    }



    /// <summary>
    /// 响应管理类
    /// 
    /// </summary>
    public class ResponserManager
    {
        public SocketServer Server { private set; get; }
        public ResponserManager(SocketServer server)
        {
            Responsers = new Dictionary<string, Type>();
            usePool = true;
            this.Server = server;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useThreadPool"></param>
        public ResponserManager(bool useThreadPool, SocketServer server)
            : this(server)
        {
            usePool = useThreadPool;
        }

        private bool usePool = false;

        /// <summary>
        /// 响应
        /// </summary>
        /// <param name="request"></param>
        /// <param name="client"></param>
        public void DoReponse(Request request, Client client)
        {
            if (Responsers.ContainsKey(request.API))
            {
                var state = new RequestState
                {
                    Client = client,
                    Request = request,
                    Responser = Responsers[request.API]
                };
#if DEBUG
                lock (syncroot)
                    CurrentRequeters++;
#endif
                if (usePool)
                {
                    if (this.Server.IsWorking)
                    {
                        if (!client.IsClose)
                        {
                            if (client.Worker == null) 
                                WorkResponse(state);
                            else 
                                client.Worker.QueueWorkItem(new AThreadPool.ThreadCallBack(WorkResponse), state);
                        }
                    }
                    else
                    {
                        Utility.Debuger.DebugLog("Server is stoped!");
                    }
                }
                else
                {
                    WorkResponse(state);
                }
            }
        }

        private object WorkResponse(object state)
        {
#if DEBUG
            var begin = DateTime.Now;
#endif
            RequestState stateObj = state as RequestState;
            var request = stateObj.Request;
            var client = stateObj.Client;
            var responser = Activator.CreateInstance(stateObj.Responser) as Responser;
            if (responser == null)
            {
                Utility.Debuger.LogWaring(string.Format("API:{0} No Responser", request.API));
                return false;
            }
            responser.Client = client;
            responser.Request = request;
            responser.ResponserManager = this;


            //响应
            try
            {
				var admission = true;
				if(responser.RequireAdmission)
				{
					admission=client.HaveAdmission; 
				}
				if(admission)
                    responser.DoRequest(request);
			    else
				{	
					responser.Success  =false;
					responser.ErrorCode =  ResponserErrorCode.NoAdminssion;
				}

			}
            catch (Exception ex)
            {
                responser.HandleException = ex;
                Utility.Debuger.LogError(string.Format("Handle: API:[{0}]\n, ex:{1}",request.API, ex));
            }
            SendResponser(client, responser.Success, responser.Result, request.API, request.RequesterID);

#if DEBUG
            //在调试的时候模式下看消耗时间
            
                Xxp.Libs.Utility.Debuger.DebugLog(string.Format("API:{0} RequestID:{1} Params:{2} Cost:{3}",
                request.API,
                request.RequesterID,
                request.ToString(),
                DateTime.Now - begin));
             
                lock (syncroot)
                    CurrentRequeters--;
#endif
            
            return true;
        }

        private class RequestState
        {
            public Request Request { set; get; }
            public Client Client { set; get; }
            public Type Responser { set; get; }
        }

        private Dictionary<string, Type> Responsers { set; get; }
        /// <summary>
        /// 添加一个API响应
        /// </summary>
        /// <param name="Api"></param>
        /// <param name="type"></param>
        public void AddResponser(string Api, Type type)
        {
            if (Responsers.ContainsKey(Api))
            {
                throw new Exception("API" + Api + "exists!");
            }
            Responsers.Add(Api, type);
        }

        /// <summary>
        /// 发送一个服务器响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="success"></param>
        /// <param name="result"></param>
        /// <param name="api"></param>
        /// <param name="requestID"></param>
        private void SendResponser(Client client, bool success, string result, string api, long requestID)
        {
            var bytes = new ResponserResult
                    {
                        RequesterID = requestID,
                        Result = result,
                        Success = success,
                        API = api
                    }.ToBinary();
            var message = new Message((int)MessageClass.Response, 0, bytes);
            client.SendMessage(message);
        }

        /// <summary>
        /// 发送一个 主动响应消息
        /// 无客户端请求的情况下
        /// 用于服务器发送主动更新
        /// </summary>
        /// <param name="client"></param>
        /// <param name="result"></param>
        /// <param name="api"></param>
        public void SendResponseToListener(Client client, string result, string api)
        {
            SendResponser(client, true, result, api, 0);
        }

		/// <summary>
		/// Servers the response.
		/// </summary>
		/// <param name="api">API.</param>
		/// <param name="paramaters">Paramaters.</param>
		/// <param name="client">Client.</param>
		public void ServerResponse(string api, List<Paramater> paramaters ,Client client)
		{
			ServerResponse (api, paramaters, client, null);
		}

		public void ServerResponse<T>( List<Paramater> paramaters, Client client) where T: Responser, new()
		{
			ServerResponse<T> (paramaters, client, null);
		}

		public void ServerResponse<T>( List<Paramater> paramaters, Client client,object userState)where T: Responser, new()
		{
			var att = 
				(typeof(T)).GetCustomAttributes(typeof(Xxp.Libs.Net.ResponserName), false)[0] 
			     as Xxp.Libs.Net.ResponserName;
			if (att == null)
				return;
			ServerResponse (att.Api, paramaters, client, userState);
		}

		public void ServerResponse(string api, List<Paramater> paramaters, Client client, object userState)
		{
			var request = new Request (-1);
			request.API = api;
			request.Paramaters = paramaters;
			request.RequesterID = -1;
			request.UserState = userState;
			this.DoReponse (request, client);
		}

#if DEBUG
        private object syncroot = new object();
#endif
        public volatile int CurrentRequeters = 0;

        /// <summary>
        /// 注册一个程序集里面的响应者
        /// </summary>
        /// <param name="assmbly"></param>
        public void RegisterResponser(Assembly assmbly)
        {
            var responsers = assmbly.GetTypes().Where(t => t.BaseType == typeof(Xxp.Libs.Net.Responser));
            foreach (var r in responsers)
            {
                var att = r.GetCustomAttributes(typeof(Xxp.Libs.Net.ResponserName), false)[0] as Xxp.Libs.Net.ResponserName;
                if (att == null)
                    continue;
                AddResponser(att.Api, r);
            }
        }
    }

    /// <summary>
    /// 请求响应
    /// @author:xxp
    /// </summary>
    public abstract class Responser
    {
        public abstract void DoRequest(Request request);
        /// <summary>
        /// 当前请求的客户端
        /// </summary>
        public Client Client { set; get; }
        /// <summary>
        /// 请求对象
        /// </summary>
        public Request Request { set; get; }
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool Success { set; get; }
        /// <summary>
        /// 请求数据
        /// </summary>
        public string Result { set; get; }


		public ResponserErrorCode ErrorCode{ set; get; }

        public Responser()
        {
            Success = false;
			RequireAdmission = true;
        }
        public ResponserManager ResponserManager { get; set; }
        public Exception HandleException { set; get; }
		public bool RequireAdmission{ set; get; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ResponserName : Attribute
    {
        public ResponserName(string api)
        {
            Api = api;
        }
        public string Api { set; get; }
    }

	public enum ResponserErrorCode{
		None,
		NoAdminssion = 0x001
	}

}
