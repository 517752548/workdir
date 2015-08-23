using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class HttpManager:UnityEngine.MonoBehaviour
    {

        private static HttpManager _m;
        public static HttpManager Current
        {
            get
            {
                if (_m == null)
                {
                    var gamebj = new GameObject("___http_manager____", typeof(HttpManager));
                    DontDestroyOnLoad(gamebj);
                }

                return _m;
            }
        }
        public void Start() {
            IsWorking = true;
            StartCoroutine(Worker());
        }
        public void Awake() {
            _m = this;
        }
        public void OnDestory() {
            _m = null;
        }

        public Request CreateRequest(string url, HttpCallBack callBack, WWWForm form = null)
        {
            var r = new Request(url, callBack);
            r.Form = form;
            r.RequestID = GetNewID();
            _request.Enqueue(r);
            return r;
        }

        private static int _requestID = 1;
        private int GetNewID()
        {
            _requestID++;
            if (_requestID == int.MaxValue) _requestID = 1;
            return _requestID;
        }

        private Queue<Request> _request = new Queue<Request>();

        private bool IsWorking = false;

        public const int TIME_OUT = 30;

        public const int MAX_RE_TRY_TIMES = 3;
        public  IEnumerator Worker()
        {
            while(IsWorking)
            {
                if(_request.Count>0)
                {
                    var r = _request.Dequeue();
                    WWW www;
                    var url = r.GetURL();

                    Debug.Log(url);

                    if(r.Form !=null)
                    {
                        www = new WWW(url, r.Form);
                    }
                    else
                    {
                        www = new WWW(url);
                    }
                   
                    float startTime = Time.realtimeSinceStartup;
                    while(!www.isDone)
                    {
                        if (startTime + TIME_OUT <= Time.realtimeSinceStartup)
                        {
                            break;
                        }
                        yield return null;
                    }


                    if (www.isDone)
                    {
                        r.CallBack(www, !string.IsNullOrEmpty(www.error) ? ResultCode.Error : ResultCode.Success,r);
                    }
                    else
                    {
                        //www.Dispose();
                        if (r.ReTryTime++ > MAX_RE_TRY_TIMES)
                        {
                            r.CallBack(null, ResultCode.TimeOut,r);
                        }
                        else
                        {
                            _request.Enqueue(r);
                        }
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }
    }
    public enum ResultCode
    {
        Success,
        TimeOut,
        Error
    }
    public class Request
    {
        public Request(string url, HttpCallBack callBack)
        {
            Url = url;
            CallBack = callBack;
            Params = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Params { set; get; }
        public int RequestID { set; get; }
        private string Url { set; get; }

        public UnityEngine.WWWForm Form { set; get; }

        public int ReTryTime { set; get; }

        public HttpCallBack CallBack;

        public string GetURL()
        {
            string parms = string.Empty;
            if (Params.Count > 0)
            {
                var sb = new StringBuilder();
                bool isFirst = true;
                foreach (var i in Params)
                {
                    if (!isFirst)
                    {
                        sb.Append("&");
                    }
                    sb.Append(WWW.EscapeURL(i.Key));
                    sb.Append("=");
                    sb.Append(WWW.EscapeURL(i.Value));
                    isFirst = false;
                }
            }
            return Url + (string.IsNullOrEmpty(parms) ? ("?timestamp=" + DateTime.Now.Ticks) : (parms + "&timestamp=" + DateTime.Now.Ticks));
        }
    }

    public delegate void HttpCallBack(WWW www,ResultCode code,Request request);
}
