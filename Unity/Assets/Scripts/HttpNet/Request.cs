using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HttpNet
{
    public class Request
    {
        public string JsonPost { set; get; }

        public string JsonResult { set; get; }

        public int RequestID { set; get; }

        public Action<Request> CallBack;

    }

    public class RequestManager : Tools.XSingleton<RequestManager>
    {
        public static Request CreateRequest(string json)
        {
            return null;

        }
    }
}
